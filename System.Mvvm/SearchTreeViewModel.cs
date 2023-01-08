using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Mvvm.Model;
using System.Reflection;
using System.Text;

namespace System.Mvvm
{
	/// <summary>
	/// Base view model for searchable tree view
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="T2">The type of the 2.</typeparam>
	/// <seealso cref="System.Mvvm.SearchViewModel&lt;T, T2&gt;" />
	public abstract class SearchTreeViewModel<T, T2> : SearchViewModel<T, T2> where T2 : IEnumerable<T>, new()
    {

        #region Properties

        private string[] _groupingNames;

		/// <summary>
		/// Gets or sets the grouping names.
		/// </summary>
		/// <value>The grouping names.</value>
		public string[] GroupingNames
        {
            get { return _groupingNames; }
            set { _groupingNames = value; }
        }


		/// <summary>
		/// Gets the tree path.
		/// </summary>
		/// <value>The tree path.</value>
		public virtual IReadOnlyCollection<TreeViewObject> TreePath
        {
            get
            {

                NotifyPropertyChanged("ItemCount");

                return BuildTreePath(Items);
            }
        }

		#endregion

		#region Constructors


		/// <summary>
		/// Initializes a new instance of the <see cref="SearchTreeViewModel{T, T2}"/> class.
		/// </summary>
		/// <param name="searchText">The search text.</param>
		public SearchTreeViewModel(string searchText = null) : base(searchText)
        {
            WhenPropertyChanged(nameof(Items), () => NotifyPropertyChanged("TreePath"));
        }
		#endregion

		#region Methods

		/// <summary>
		/// Builds the tree path.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns>IReadOnlyCollection&lt;TreeViewObject&gt;.</returns>
		private IReadOnlyCollection<TreeViewObject> BuildTreePath(IEnumerable<T> data)
        {
            return BuildPath(data, GroupingNames);
        }

		/// <summary>
		/// Builds the path.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="groupNames">The group names.</param>
		/// <returns>IReadOnlyCollection&lt;TreeViewObject&gt;.</returns>
		private IReadOnlyCollection<TreeViewObject> BuildPath(IEnumerable<T> data, string[] groupNames)
        {
            var groupby2 = groupNames[1];

            if (data == null || data.Count() == 0)
                return null;

            var dataStruct = new List<TreeViewItemModel>();

            foreach (var aKey in data)
            {
                var groupBy = groupNames[0];
                var item = BuildTree(dataStruct, aKey, groupBy);

                if (groupNames.Length > 1)
                {
                    for (var loop = 1; loop < groupNames.Length; loop++)
                    {
                        item = BuildTree(item.Children, aKey, groupNames[loop]);
                    }
                }
                
            }

            var treeItems = new List<TreeViewObject>();

            foreach (var aItem in dataStruct)
                treeItems.Add(new TreeViewObject(aItem));

            var rootFolderVMs = new ReadOnlyCollection<TreeViewObject>(treeItems);

            return rootFolderVMs;
        }

		/// <summary>
		/// Builds the tree.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="dataItem">The data item.</param>
		/// <param name="groupByName">Name of the group by.</param>
		/// <returns>TreeViewItemModel.</returns>
		/// <exception cref="System.Exception">Cannot find property named {groupByName}</exception>
		private TreeViewItemModel BuildTree(List<TreeViewItemModel> data, T dataItem, string groupByName)
        {
            var aProp = dataItem.GetType().GetTypeInfo().GetDeclaredProperty(groupByName);

            if (aProp == null)
                throw new Exception($"Cannot find property named {groupByName}");

            var propValue = aProp.GetValue(dataItem);

            var product = propValue?.ToString() ?? "Unknown";

            var item = data.FirstOrDefault(x => x.Name.Equals(product));

            if (item == null)
            {
                item = new TreeViewItemModel()
                {
                    Name = product,
                };

                data.Add(item);
            }

            return item;
        }
        #endregion
    }
}
