using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections.Generic
{
	public class Set<T> : IEnumerable<T>
	{
		private Dictionary<T,byte> items;

		public Set()
		{
			items = new Dictionary<T,byte>();
		}
		public Set(int capacity)
		{
			items = new Dictionary<T,byte>(capacity);
		}

		public void Add(T element)
		{
			if (!items.ContainsKey(element))
				items.Add(element, 0);
		}
		public bool Remove(T element)
		{
			return items.Remove(element);
		}
		public bool Contains(T element)
		{
			return items.ContainsKey(element);
		}
		public void Clear()
		{
			items.Clear();
		}
		public T[] ToArray()
		{
			T[] arr = new T[items.Keys.Count];
			items.Keys.CopyTo(arr, 0);
			return arr;
		}

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return items.Keys.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return items.Keys.GetEnumerator();
		}

		#endregion
	}
}
