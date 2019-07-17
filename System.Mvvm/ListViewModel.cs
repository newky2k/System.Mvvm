using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace System.Mvvm
{
    public abstract class ListViewModel<T, T2> : ViewModel where T2 : IEnumerable<T>, new()
    {
        #region Fields
        private T selectedItem;
        protected int itemCount;
        private T2 items;
        private T2 selectedItems;
        #endregion

        #region Events

        public event EventHandler<T> SelectedItemChanged = delegate { };

        public event EventHandler OnDataRefreshed = delegate { };

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the selected items.
        /// </summary>
        /// <value>The selected items.</value>
        public virtual T2 SelectedItems
        {
            get
            {
                return selectedItems;
            }
            set
            {
                selectedItems = value;
                NotifyPropertyChanged("SelectedItems");
            }
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public virtual T2 Items
        {
            get
            {
                if (items == null)
                    items = new T2();

                itemCount = items.Count();

                return items;

            }
            set
            {
                items = value;

                NotifyPropertyChanged("Items");
                NotifyPropertyChanged("ItemCount");
            }
        }

        public virtual string ItemCount
        {
            get
            {
                var msg = (itemCount == 1) ? "item" : "items";

                return String.Format("Found {0} {1}", itemCount, msg);
            }
        }

        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        /// <value>
        /// The refresh command.
        /// </value>
        public virtual ICommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    await RefreshAsync();
                });
            }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public virtual T SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                NotifyPropertyChanged("SelectedItem");
                SelectedItemChanged(this, value);
                HandleItemSelection(value);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSearchViewModel"/> class.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        public ListViewModel()
        {
            
        }
        #endregion

        #region Methods

        /// <summary>
        /// Reloads the data asynchronously
        /// </summary>
        /// <returns>Task.</returns>
        public abstract Task RefreshAsync();

        public void DidRefreshData()
        {
            OnDataRefreshed(this, new EventArgs());

        }

        protected virtual void HandleItemSelection(T selection)
        {

        }
        #endregion
    }
}
