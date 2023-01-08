using System;
using System.Collections.Generic;
using System.Text;

namespace System.Mvvm.Model
{
	/// <summary>
	/// View model base class for Tree view objects
	/// </summary>
	public class TreeViewItemModel
    {
		/// <summary>
		/// Gets or sets the display name for the item
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>The data.</value>
		public object Data { get; set; }

		/// <summary>
		/// Gets or sets the children.
		/// </summary>
		/// <value>The children.</value>
		public List<TreeViewItemModel> Children { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TreeViewItemModel"/> class.
		/// </summary>
		public TreeViewItemModel()
        {
            Children = new List<TreeViewItemModel>();
        }
    }
}
