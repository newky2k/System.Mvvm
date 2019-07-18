using System;
using System.Collections.Generic;
using System.Text;

namespace System.Mvvm
{
    public abstract class SearchTreeViewModel<T, T2> : SearchViewModel<T, T2> where T2 : IEnumerable<T>, new()
    {

        #region Properties

        public virtual IReadOnlyCollection<IEnumerable<T>> TreePath
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

        protected abstract IReadOnlyCollection<IEnumerable<T>> BuildTreePath(IEnumerable<T> data);
            

        #endregion
    }
}
