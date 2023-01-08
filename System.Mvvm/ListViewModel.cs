using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace System.Mvvm
{
	/// <summary>
	/// Base view model for List views
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="T2">The type of the 2.</typeparam>
	/// <seealso cref="System.Mvvm.ViewModel" />
	public abstract class ListViewModel<T, T2> : ViewModel where T2 : IEnumerable<T>, new()
    {
        #region Fields
        private T selectedItem;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
		protected int itemCount;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
		private T2 items;
        private T2 selectedItems;
		#endregion

		#region Events

		/// <summary>
		/// Occurs when [selected item changed].
		/// </summary>
		public event EventHandler<T> SelectedItemChanged = delegate { };

		/// <summary>
		/// Occurs when [on data refreshed].
		/// </summary>
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
		/// <value>The items.</value>
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

		/// <summary>
		/// Gets the item count.
		/// </summary>
		/// <value>The item count.</value>
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
		/// <value>The refresh command.</value>
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
		/// Initializes a new instance of the <see cref="ListViewModel{T, T2}"/> class.
		/// </summary>
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

		/// <summary>
		/// Dids the refresh data.
		/// </summary>
		[Obsolete("Use NotifyDataRefreshed instead")]
		public void DidRefreshData() => NotifyDataRefreshed();

		/// <summary>
		/// Notifies that the data has been refreshed.
		/// </summary>
		public void NotifyDataRefreshed()
		{
			OnDataRefreshed(this, new EventArgs());

		}

		/// <summary>
		/// Handles the item selection.
		/// </summary>
		/// <param name="selection">The selection.</param>
		protected virtual void HandleItemSelection(T selection)
        {

        }
        #endregion
    }
}
