using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace DSoft.System.Mvvm.SourceGenerators
{
	/// <summary>
	/// Class ViewModelGenerator.
	/// Implements the <see cref="ISourceGenerator" />
	/// </summary>
	/// <seealso cref="ISourceGenerator" />
	[Generator]
    public class ViewModelGenerator : ISourceGenerator
    {
        private const string attributeText = @"
using System;
namespace System.Mvvm
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    [System.Diagnostics.Conditional(""MVVMViewModelAttribute_DEBUG"")]
    public sealed class MVVMViewModelAttribute : Attribute
    {
        public MVVMViewModelAttribute()
        {

        }

    }
}
";

		/// <summary>
		/// Called before generation occurs. A generator can use the <paramref name="context" />
		/// to register callbacks required to perform generation.
		/// </summary>
		/// <param name="context">The <see cref="T:Microsoft.CodeAnalysis.GeneratorInitializationContext" /> to register callbacks on</param>
		public void Initialize(GeneratorInitializationContext context)
        {

#if DEBUG
            //Debugger.Launch();
#endif
            // Register the attribute source
            context.RegisterForPostInitialization((i) => i.AddSource("MVVMViewModelAttribute", attributeText));

            // Register a syntax receiver that will be created for each generation pass
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

		/// <summary>
		/// Called to perform source generation. A generator can use the <paramref name="context" />
		/// to add source files via the <see cref="M:Microsoft.CodeAnalysis.GeneratorExecutionContext.AddSource(System.String,Microsoft.CodeAnalysis.Text.SourceText)" />
		/// method.
		/// </summary>
		/// <param name="context">The <see cref="T:Microsoft.CodeAnalysis.GeneratorExecutionContext" /> to add source to</param>
		/// <remarks>This call represents the main generation step. It is called after a <see cref="T:Microsoft.CodeAnalysis.Compilation" /> is
		/// created that contains the user written code.
		/// A generator can use the <see cref="P:Microsoft.CodeAnalysis.GeneratorExecutionContext.Compilation" /> property to
		/// discover information about the users compilation and make decisions on what source to
		/// provide.</remarks>
		public void Execute(GeneratorExecutionContext context)
        {
            // retrieve the populated receiver 
            if (!(context.SyntaxContextReceiver is SyntaxReceiver receiver))
                return;

            // get the added attribute, and INotifyPropertyChanged
            INamedTypeSymbol attributeSymbol = context.Compilation.GetTypeByMetadataName("System.Mvvm.MVVMViewModelAttribute");
            INamedTypeSymbol notifySymbol = context.Compilation.GetTypeByMetadataName("System.Mvvm.ViewModel");

            string classSource = ProcessClass(receiver.ClassTypeSymbol, receiver.CommandFields, attributeSymbol, notifySymbol, context, receiver.Properties);
            context.AddSource($"{receiver.ClassTypeSymbol}_viewModel.cs", SourceText.From(classSource, Encoding.UTF8));

            // group the fields by class, and generate the source
            //foreach (IGrouping<INamedTypeSymbol, IFieldSymbol> group in receiver.Fields.GroupBy<IFieldSymbol, INamedTypeSymbol>(f => f.ContainingType, SymbolEqualityComparer.Default))
            //{
            //    string classSource = ProcessClass(group.Key, group.ToList(), attributeSymbol, notifySymbol, context);
            //    context.AddSource($"{group.Key.Name}_viewModel.cs", SourceText.From(classSource, Encoding.UTF8));
            //}
        }

		/// <summary>
		/// Processes the class.
		/// </summary>
		/// <param name="classSymbol">The class symbol.</param>
		/// <param name="fields">The fields.</param>
		/// <param name="attributeSymbol">The attribute symbol.</param>
		/// <param name="notifySymbol">The notify symbol.</param>
		/// <param name="context">The context.</param>
		/// <param name="Properties">The properties.</param>
		/// <returns>System.String.</returns>
		private string ProcessClass(INamedTypeSymbol classSymbol, List<IFieldSymbol> fields, ISymbol attributeSymbol, ISymbol notifySymbol, GeneratorExecutionContext context, List<IPropertySymbol> Properties)
        {
            if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            {
                return null; //TODO: issue a diagnostic that it must be top level
            }

            var baseTypeText = string.Empty;

            if (classSymbol.BaseType != null)
            {
                var baseType = classSymbol.BaseType.ToDisplayString();

                if (baseType.Equals("object"))
                {
                    baseTypeText = $": {notifySymbol.ToDisplayString()}";
                }
            }

            string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();

            // begin building the generated source
            StringBuilder source = new StringBuilder($@"using System.Linq;
using System.Mvvm;
using System.Collections.Generic;

namespace {namespaceName}
{{
    public partial class {classSymbol.Name} {baseTypeText}
    {{
");

#if DEBUG

            source.Append($@"        public bool IsDebug {{ get; set;}}");

#endif

            if (Properties.Count > 0)
            {
                source.Append($@"
        protected override void NotifyCommandsPropertiesChanged()
        {{");
                //create properties for each field
                foreach (IPropertySymbol propertySymbol in Properties)
                {
                    string propName = propertySymbol.Name;

                    source.Append($@"
            SimpleNotififyPropertyChanged(""{propName}"");");

                }

                source.Append($@"
        }}");

                source.AppendLine();

            }

            if (fields.Count > 0)
            {
                source.Append($@"
        protected override void NotifyCommandFieldsCanExecuteChanged()
        {{
            var dCommands = new List<DelegateCommand>();");

                //create properties for each field
                foreach (IFieldSymbol fieldSymbol in fields)
                {
                    string fieldName = fieldSymbol.Name;

                    source.Append($@"

            if ({fieldName} != null)
                dCommands.Add({fieldName});");

                }

                source.Append($@"

            if (dCommands.Any())
                DelegateCommand.BulkNotifyRaiseCanExecuteChanged(dCommands);
        }}");
            }

            source.Append($@"
    }}
}}");

            string sourceString = source.ToString();

            return sourceString;
        }

		/// <summary>
		/// Created on demand before each generation pass
		/// </summary>
		class SyntaxReceiver : ISyntaxContextReceiver
        {
			/// <summary>
			/// Gets the command fields.
			/// </summary>
			/// <value>The command fields.</value>
			public List<IFieldSymbol> CommandFields { get; } = new List<IFieldSymbol>();

			/// <summary>
			/// Gets the properties.
			/// </summary>
			/// <value>The properties.</value>
			public List<IPropertySymbol> Properties { get; } = new List<IPropertySymbol>();

			/// <summary>
			/// Gets or sets the class type symbol.
			/// </summary>
			/// <value>The class type symbol.</value>
			public INamedTypeSymbol ClassTypeSymbol { get; set; }

			/// <summary>
			/// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
			/// </summary>
			/// <param name="context">The context.</param>
			public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                if (context.Node is ClassDeclarationSyntax classDeclaration)
                {
                    INamedTypeSymbol classTypeSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

                    if (classTypeSymbol.GetAttributes().Any(ad => ad.AttributeClass.ToDisplayString() == "System.Mvvm.MVVMViewModelAttribute"))
                    {
                        ClassTypeSymbol = classTypeSymbol;

                        var props = classDeclaration.Members.OfType<PropertyDeclarationSyntax>();

                        ImmutableArray<ISymbol> members = ClassTypeSymbol.GetMembers();

                        foreach (ISymbol member in members)
                        {
                            if (member is IFieldSymbol fieldSymbol)
                            {
                                if (fieldSymbol.Type.ToDisplayString() == "System.Mvvm.DelegateCommand")
                                {
                                    CommandFields.Add(fieldSymbol);
                                }
                            }
                        }

                        

                        //check to see if any properties are exposed
                        if (props.Any())
                        {
                            var commandFieldnames = new List<string>();

                            var fieldsToRemove = new List<IFieldSymbol>();

                            //work through each property
                            foreach (PropertyDeclarationSyntax propertySyntax in props)
                            {
                               
                                IPropertySymbol propertySymbol = context.SemanticModel.GetDeclaredSymbol(propertySyntax);

                                if (propertySymbol.Type.ToDisplayString() == "System.Windows.Input.ICommand" || (propertySymbol.Type.ToDisplayString() == "System.Mvvm.DelegateCommand"))
                                {
                                    var propCode = propertySyntax.ToString();

                                    if (CommandFields.Any())
                                    {
                                        foreach (var command in CommandFields)
                                        {
                                            if (propCode.Contains(command.Name))
                                            {
                                                //don't notify the field as its cheaper to call the OnPropertyChanged notification
                                                fieldsToRemove.Add(command);
                                            }
                                        }

                                    }

                                    Properties.Add(propertySymbol);

                                }

                            }


                           //remove removed fields :-)
                           foreach (var field in fieldsToRemove)
                                CommandFields.Remove(field);
                        }


                       
                    }
                }

            }
        }
    }
}
