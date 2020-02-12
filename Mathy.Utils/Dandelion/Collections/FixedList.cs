// Dandelion.Collections.FixedList<T>
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mathy.Utils.Dandelion.Collections
{
	public class FixedList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		private class FixedListEnumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			private FixedList<T> list;

			private int position;

			public T Current => list[position];

			object IEnumerator.Current => list[position];

			public FixedListEnumerator(FixedList<T> list)
			{
				this.list = list;
				position = -1;
			}

			public void Dispose()
			{
				list = null;
				position = -1;
			}

			public bool MoveNext()
			{
				if (position == list.Count - 1)
				{
					return false;
				}
				position++;
				return true;
			}

			public void Reset()
			{
				position = -1;
			}
		}

		private T[] items;

		private int maxCount;

		private int startIndex;

		private int count;

		public T this[int index]
		{
			get
			{
				return items[(startIndex + index) % maxCount];
			}
			set
			{
				items[(startIndex + index) % maxCount] = value;
			}
		}

		public int Count => count;

		public bool IsReadOnly => false;

		public FixedList(int maxCount)
		{
			this.maxCount = maxCount;
			items = new T[maxCount];
		}

		public int IndexOf(T item)
		{
			for (int i = 0; i <= count - 1; i++)
			{
				int num = (startIndex + i) % maxCount;
				if ((object)items[num] == (object)item)
				{
					return i;
				}
			}
			return -1;
		}

		public void Insert(int index, T item)
		{
			throw new NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		public void Add(T item)
		{
			int num;
			if (count < maxCount)
			{
				count++;
				num = (startIndex + count - 1) % maxCount;
			}
			else
			{
				num = (startIndex + maxCount) % maxCount;
				startIndex = (startIndex + 1) % maxCount;
			}
			items[num] = item;
		}

		public void Clear()
		{
			count = 0;
			startIndex = 0;
			for (int i = 0; i <= maxCount - 1; i++)
			{
				items[i] = default(T);
			}
		}

		public bool Contains(T item)
		{
			for (int i = 0; i <= count - 1; i++)
			{
				int num = (startIndex + i) % maxCount;
				if ((object)items[num] == (object)item)
				{
					return true;
				}
			}
			return false;
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			if (arrayIndex + count > array.Length)
			{
				throw new ArgumentException($"Cannot copy from {arrayIndex} to {arrayIndex + count - 1} to an array of length {count}.");
			}
			int num = 0;
			for (int i = arrayIndex; i <= count - 1; i++)
			{
				int num2 = (startIndex + i) % maxCount;
				array[num] = items[num2];
				num++;
			}
		}

		public bool Remove(T item)
		{
			throw new NotImplementedException();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return GetFixedListEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetFixedListEnumerator();
		}

		private IEnumerator<T> GetFixedListEnumerator()
		{
			return new FixedListEnumerator(this);
		}
	}
}
