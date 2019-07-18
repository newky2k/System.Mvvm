using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Mvvm.Model;
using System.Reflection;
using System.Text;

namespace System.Mvvm
{
    public abstract class SearchTreeViewModel<T, T2> : SearchViewModel<T, T2> where T2 : IEnumerable<T>, new()
    {

        #region Properties

        private string[] _groupingNames;

        public string[] GroupingNames
        {
            get { return _groupingNames; }
            set { _groupingNames = value; }
        }


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
        /// Initializes a new instance of the <see cref="BaseSearchViewModel"/> class.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        public SearchTreeViewModel(string searchText = null) : base(searchText)
        {
            WhenPropertyChanged(nameof(Items), () => NotifyPropertyChanged("TreePath"));
        }
        #endregion

        #region Methods

        private IReadOnlyCollection<TreeViewObject> BuildTreePath(IEnumerable<T> data)
        {
            return BuildPath(data, GroupingNames);
        }

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
