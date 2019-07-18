using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace System.Mvvm.Model
{
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

        public TreeViewObject(TreeViewItemModel item)
            : this(item, null)
        {
        }

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

        public TreeViewItemModel Item
        {
            get { return _Item; }
        }

        public ReadOnlyCollection<TreeViewObject> Children
        {
            get { return _children; }
        }

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

        public bool NameContainsText(string text)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(this.Name))
                return false;

            return this.Name.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) > -1;
        }

        #endregion // NameContainsText

        #region Parent

        public TreeViewObject Parent
        {
            get { return _parent; }
        }

        #endregion // Parent

        #endregion // Presentation Members        

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members
    }
}
