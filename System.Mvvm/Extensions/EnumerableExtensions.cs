using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    public static class EnumerableExtensions
    {
		/// <summary>
		/// Converts the source IEnumbable<typeparamref name="T"/> object to an Observable<typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items">The items.</param>
		/// <returns></returns>
		public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> items)
        {
            var cols = new ObservableCollection<T>();

            if (items == null || items.Count() == 0)
                return cols;

            foreach (var aItem in items)
                cols.Add(aItem);

            return cols;
        }
    }
}
