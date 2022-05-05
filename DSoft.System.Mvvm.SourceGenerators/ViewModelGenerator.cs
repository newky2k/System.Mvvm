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

        public void Initialize(GeneratorInitializationContext context)
        {

            Debugger.Launch();

            // Register the attribute source
            context.RegisterForPostInitialization((i) => i.AddSource("MVVMViewModelAttribute", attributeText));

            // Register a syntax receiver that will be created for each generation pass
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // retrieve the populated receiver 
            if (!(context.SyntaxContextReceiver is SyntaxReceiver receiver))
                return;

            // get the added attribute, and INotifyPropertyChanged
            INamedTypeSymbol attributeSymbol = context.Compilation.GetTypeByMetadataName("System.Mvvm.MVVMViewModelAttribute");
            INamedTypeSymbol notifySymbol = context.Compilation.GetTypeByMetadataName("System.Mvvm.ViewModel");

            string classSource = ProcessClass(receiver.ClassTypeSymbol, receiver.Fields, attributeSymbol, notifySymbol, context);
            context.AddSource($"{receiver.ClassTypeSymbol}_viewModel.cs", SourceText.From(classSource, Encoding.UTF8));

            // group the fields by class, and generate the source
            //foreach (IGrouping<INamedTypeSymbol, IFieldSymbol> group in receiver.Fields.GroupBy<IFieldSymbol, INamedTypeSymbol>(f => f.ContainingType, SymbolEqualityComparer.Default))
            //{
            //    string classSource = ProcessClass(group.Key, group.ToList(), attributeSymbol, notifySymbol, context);
            //    context.AddSource($"{group.Key.Name}_viewModel.cs", SourceText.From(classSource, Encoding.UTF8));
            //}
        }

        private string ProcessClass(INamedTypeSymbol classSymbol, List<IFieldSymbol> fields, ISymbol attributeSymbol, ISymbol notifySymbol, GeneratorExecutionContext context)
        {
            if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            {
                return null; //TODO: issue a diagnostic that it must be top level
            }

            string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();

            // begin building the generated source
            StringBuilder source = new StringBuilder($@"using System.Linq;
using System.Mvvm;
using System.Collections.Generic;

namespace {namespaceName}
{{
    public partial class {classSymbol.Name} : {notifySymbol.ToDisplayString()}
    {{
");

            // if the class doesn't implement INotifyPropertyChanged already, add it
            //if (!classSymbol.Interfaces.Contains(notifySymbol, SymbolEqualityComparer.Default))
            //{
            //    ;// source.Append("public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;");
            //}


            if (fields.Count > 0)
            {
                source.Append($@"
        protected override void NotifyCommandsCanExecuteChanged()
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
            public List<IFieldSymbol> Fields { get; } = new List<IFieldSymbol>();

            public INamedTypeSymbol ClassTypeSymbol { get; set; }

            /// <summary>
            /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
            /// </summary>
            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                if (context.Node is ClassDeclarationSyntax classDeclaration)
                {
                    INamedTypeSymbol classTypeSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

                    if (classTypeSymbol.GetAttributes().Any(ad => ad.AttributeClass.ToDisplayString() == "System.Mvvm.MVVMViewModelAttribute"))
                    {
                        ClassTypeSymbol = classTypeSymbol;


                        ImmutableArray<ISymbol> members = ClassTypeSymbol.GetMembers();

                        foreach (ISymbol member in members)
                        {
                            if (member is IFieldSymbol fieldSymbol)
                            {
                                if (fieldSymbol.Type.ToDisplayString() == "System.Mvvm.DelegateCommand")
                                {
                                    Fields.Add(fieldSymbol);
                                }
                            }
                        }
                    }
                }
                //// any field with at least one attribute is a candidate for property generation
                //if (context.Node is FieldDeclarationSyntax fieldDeclarationSyntax
                //    && fieldDeclarationSyntax.AttributeLists.Count > 0)
                //{
                //    foreach (VariableDeclaratorSyntax variable in fieldDeclarationSyntax.Declaration.Variables)
                //    {
                //        // Get the symbol being declared by the field, and keep it if its annotated
                //        IFieldSymbol fieldSymbol = context.SemanticModel.GetDeclaredSymbol(variable) as IFieldSymbol;
                //        if (fieldSymbol.GetAttributes().Any(ad => ad.AttributeClass.ToDisplayString() == "AutoNotify.AutoNotifyAttribute"))
                //        {
                //            Fields.Add(fieldSymbol);
                //        }
                //    }
                //}
            }
        }
    }
}
