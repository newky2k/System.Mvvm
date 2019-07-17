using System;
using System.Collections.Generic;
using System.Text;

namespace System.Mvvm
{
    public abstract class SearchTreeViewModel<T, T2> : SearchViewModel<T, List<T>>
    {

        #region Properties

        public virtual IReadOnlyCollection<T2> TreePath
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

        protected abstract IReadOnlyCollection<T2> BuildTreePath(List<T> data);


        #endregion
    }
}
