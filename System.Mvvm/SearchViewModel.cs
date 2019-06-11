using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace System.Mvvm
{
    public abstract class SearchViewModel<T, T2> : ListViewModel<T,T2> where T2 : IEnumerable<T>, new()
    {
        #region Fields
        private String searchText;
        #endregion

        #region Events

        public event EventHandler<T> ItemDoubleClicked = delegate { };

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public override T2 Items
        {
            get
            {
                var results = ApplyFilter(base.Items);

                itemCount = results.Count();

                return results;

            }
            set
            {
                base.Items = value;

            }
        }

        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>
        /// The search text.
        /// </value>
        public String SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;

                NotifyPropertyChanged("SearchText");
                NotifyPropertyChanged("Items");
                NotifyPropertyChanged("ItemCount");
            }
        }

        /// <summary>
        /// Gets the clear search command.
        /// </summary>
        /// <value>
        /// The clear search command.
        /// </value>
        public ICommand ClearSearchCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    SearchText = null;
                    SelectedItem = default(T);
                });
            }
        }

        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        /// <value>
        /// The refresh command.
        /// </value>
        public override ICommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    SearchText = null;

                    await RefreshAsync();
                });
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSearchViewModel"/> class.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        public SearchViewModel(string searchText = null)
        {
            this.searchText = searchText;
        }
        #endregion

        #region Methods

        protected abstract T2 ApplyFilter(T2 data);

        public virtual void DidDoubleClick(T item)
        {
            ItemDoubleClicked(this, item);
        }


        #endregion
    }
}
