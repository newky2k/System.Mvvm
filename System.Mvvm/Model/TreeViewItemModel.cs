using System;
using System.Collections.Generic;
using System.Text;

namespace System.Mvvm.Model
{
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
