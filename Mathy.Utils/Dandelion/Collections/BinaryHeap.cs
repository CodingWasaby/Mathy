// Dandelion.Collections.BinaryHeap<T>
using System;
using System.Collections.Generic;

namespace Mathy.Utils.Dandelion.Collections
{
	public class BinaryHeap<T> where T : IBinaryHeapItem
	{
		private const int INITIAL_CAPACITY = 20;

		private object[] items;

		private int capacity;

		private IComparer<T> comparator;

		public int Size
		{
			get;
			private set;
		}

		public BinaryHeap(IComparer<T> comparator)
		{
			Reset();
			this.comparator = comparator;
		}

		public object[] GetArray()
		{
			return items;
		}

		private void EnsureCapacity(int minCapacity)
		{
			if (capacity < minCapacity)
			{
				capacity = minCapacity + (minCapacity >> 1);
				object[] destinationArray = new object[capacity];
				Array.Copy(items, destinationArray, items.Length);
				items = destinationArray;
			}
		}

		private void FilterUp(int index)
		{
			int num = index;
			T val = (T)items[index];
			while (num > 1)
			{
				int num2 = num >> 1;
				T val2 = (T)items[num2];
				if (comparator.Compare(val, val2) >= 0)
				{
					break;
				}
				items[num2] = val;
				val.BinaryHeapItemIndex = num2;
				items[num] = val2;
				val2.BinaryHeapItemIndex = num;
				num = num2;
			}
		}

		private void FilterDown(int index)
		{
			int num = index;
			T val = (T)items[index];
			while (true)
			{
				bool flag = true;
				int num2 = num << 1;
				if (num2 > Size)
				{
					break;
				}
				int num3 = num2 + 1;
				int num4 = (num3 > Size || comparator.Compare((T)items[num2], (T)items[num3]) <= 0) ? num2 : num3;
				T val2 = (T)items[num4];
				if (comparator.Compare(val, val2) <= 0)
				{
					break;
				}
				items[num] = val2;
				val2.BinaryHeapItemIndex = num;
				items[num4] = val;
				val.BinaryHeapItemIndex = num4;
				num = num4;
			}
		}

		public void Add(T item)
		{
			Size++;
			EnsureCapacity(Size + 1);
			items[Size] = item;
			item.BinaryHeapItemIndex = Size;
			FilterUp(Size);
		}

		public T First()
		{
			return (Size == 0) ? default(T) : ((T)items[1]);
		}

		public T PollFirst()
		{
			if (Size == 0)
			{
				return default(T);
			}
			T result = (T)items[1];
			T val = (T)items[Size];
			items[1] = val;
			val.BinaryHeapItemIndex = 1;
			items[Size] = null;
			Size--;
			FilterDown(1);
			result.BinaryHeapItemIndex = 0;
			return result;
		}

		public void Update(T item)
		{
			int binaryHeapItemIndex = item.BinaryHeapItemIndex;
			if (binaryHeapItemIndex > 1 && comparator.Compare(item, (T)items[binaryHeapItemIndex >> 1]) < 0)
			{
				FilterUp(binaryHeapItemIndex);
			}
			else
			{
				FilterDown(binaryHeapItemIndex);
			}
		}

		private void Reset()
		{
			capacity = 20;
			items = new object[20];
			Size = 0;
		}

		public void Clear()
		{
			for (int i = 1; i <= Size; i++)
			{
				items[i] = null;
			}
			Size = 0;
		}

		public void Remove(T item)
		{
			int binaryHeapItemIndex = item.BinaryHeapItemIndex;
			T val = (T)items[Size];
			items[binaryHeapItemIndex] = val;
			val.BinaryHeapItemIndex = binaryHeapItemIndex;
			items[Size] = null;
			Size--;
			if (item.BinaryHeapItemIndex != Size + 1)
			{
				Update(item);
			}
			item.BinaryHeapItemIndex = 0;
		}
	}
}
