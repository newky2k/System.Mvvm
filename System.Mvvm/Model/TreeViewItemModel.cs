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
        public string Name { get; set; }

        public object Data { get; set; }

        public List<TreeViewItemModel> Children { get; set; }

        public TreeViewItemModel()
        {
            Children = new List<TreeViewItemModel>();
        }
    }
}
