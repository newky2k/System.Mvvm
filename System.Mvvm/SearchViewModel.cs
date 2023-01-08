using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace System.Mvvm
{
	/// <summary>
	/// Base view model for searchable view
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="T2">The type of the 2.</typeparam>
	/// <seealso cref="System.Mvvm.ListViewModel&lt;T, T2&gt;" />
	public abstract class SearchViewModel<T, T2> : ListViewModel<T,T2> where T2 : IEnumerable<T>, new()
    {
        #region Fields
        private String searchText;
		#endregion

		#region Events

		/// <summary>
		/// Occurs when [item double clicked].
		/// </summary>
		public event EventHandler<T> ItemDoubleClicked = delegate { };

		#endregion

		#region Properties

		/// <summary>
		/// Gets the unfiltered items.
		/// </summary>
		/// <value>The unfiltered items.</value>
		public T2 UnfilteredItems => base.Items;

		/// <summary>
		/// Gets or sets the items.
		/// </summary>
		/// <value>The items.</value>
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
		/// <value>The search text.</value>
		public virtual string SearchText
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
		/// <value>The clear search command.</value>
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
		/// <value>The refresh command.</value>
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
		/// Initializes a new instance of the <see cref="SearchViewModel{T, T2}"/> class.
		/// </summary>
		/// <param name="searchText">The search text.</param>
		public SearchViewModel(string searchText = null)
        {
            this.searchText = searchText;
        }
		#endregion

		#region Methods

		/// <summary>
		/// Applies the filter.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns>T2.</returns>
		protected abstract T2 ApplyFilter(T2 data);

		/// <summary>
		/// Dids the double click.
		/// </summary>
		/// <param name="item">The item.</param>
		public virtual void DidDoubleClick(T item)
        {
            ItemDoubleClicked(this, item);
        }


        #endregion
    }
}
