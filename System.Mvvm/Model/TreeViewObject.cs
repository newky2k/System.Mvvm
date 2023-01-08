using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace System.Mvvm.Model
{
	/// <summary>
	/// Tree View Object.
	/// Implements the <see cref="INotifyPropertyChanged" />
	/// </summary>
	/// <seealso cref="INotifyPropertyChanged" />
	public class TreeViewObject : INotifyPropertyChanged
    {
        #region Data

        readonly ReadOnlyCollection<TreeViewObject> _children;
        readonly TreeViewObject _parent;
        readonly TreeViewItemModel _Item;

        bool _isExpanded;
        bool _isSelected;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="TreeViewObject"/> class.
		/// </summary>
		/// <param name="item">The item.</param>
		public TreeViewObject(TreeViewItemModel item)
            : this(item, null)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="TreeViewObject"/> class.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="parent">The parent.</param>
		private TreeViewObject(TreeViewItemModel item, TreeViewObject parent)
        {
            _Item = item;
            _parent = parent;

            _children = new ReadOnlyCollection<TreeViewObject>(
                    (from child in _Item.Children
                     select new TreeViewObject(child, this))
                     .ToList<TreeViewObject>());
        }

		#endregion // Constructors

		#region Folder Properties

		/// <summary>
		/// Gets the item.
		/// </summary>
		/// <value>The item.</value>
		public TreeViewItemModel Item
        {
            get { return _Item; }
        }

		/// <summary>
		/// Gets the children.
		/// </summary>
		/// <value>The children.</value>
		public ReadOnlyCollection<TreeViewObject> Children
        {
            get { return _children; }
        }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
        {
            get { return _Item.Name; }
        }

		#endregion // Person Properties

		#region Presentation Members

		#region IsExpanded

		/// <summary>
		/// Gets/sets whether the TreeViewItem
		/// associated with this object is expanded.
		/// </summary>
		/// <value><c>true</c> if this instance is expanded; otherwise, <c>false</c>.</value>
		public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (_isExpanded && _parent != null)
                    _parent.IsExpanded = true;
            }
        }

		#endregion // IsExpanded

		#region IsSelected

		/// <summary>
		/// Gets/sets whether the TreeViewItem
		/// associated with this object is selected.
		/// </summary>
		/// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
		public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

		#endregion // IsSelected

		#region NameContainsText

		/// <summary>
		/// Names the contains text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool NameContainsText(string text)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(this.Name))
                return false;

            return this.Name.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) > -1;
        }

		#endregion // NameContainsText

		#region Parent

		/// <summary>
		/// Gets the parent.
		/// </summary>
		/// <value>The parent.</value>
		public TreeViewObject Parent
        {
            get { return _parent; }
        }

		#endregion // Parent

		#endregion // Presentation Members        

		#region INotifyPropertyChanged Members

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		/// <returns></returns>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Called when [property changed].
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members
    }
}
