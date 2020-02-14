// Dandelion.Maths.PermutationIterator<T>
using Mathy.Utils.Dandelion.Maths;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mathy.Utils.Dandelion.Maths
{
	public class PermutationIterator<T> : IEnumerable<T[]>, IEnumerable
	{
		private class CombinationIteratorEnumerator<T> : IEnumerator<T[]>, IDisposable, IEnumerator
		{
			private PermutationIterator<T> owner;

			public T[] Current
			{
				get
				{
					owner.FindNext();
					return owner.current;
				}
			}

			object IEnumerator.Current => Current;

			public CombinationIteratorEnumerator(PermutationIterator<T> owner)
			{
				this.owner = owner;
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				return owner.hasNext;
			}

			public void Reset()
			{
				owner.Reset();
			}
		}

		private T[] source;

		private int[] digits;

		private int[] indexList;

		private T[] current;

		private bool hasNext;

		private int count;

		private int pos;

		public PermutationIterator(T[] source)
		{
			this.source = source;
			count = source.Length;
			digits = new int[count];
			indexList = new int[count];
			current = new T[count];
			Reset();
		}

		IEnumerator<T[]> IEnumerable<T[]>.GetEnumerator()
		{
			return new CombinationIteratorEnumerator<T>(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new CombinationIteratorEnumerator<T>(this);
		}

		private void Reset()
		{
			hasNext = true;
			pos = count - 1;
			for (int i = 0; i <= count - 1; i++)
			{
				indexList[i] = i;
				digits[i] = i;
			}
			digits[count - 1]--;
			SetCurrent();
		}

		private void SetCurrent()
		{
			for (int i = 0; i <= count - 1; i++)
			{
				current[i] = source[digits[i]];
			}
		}

		private void FindNext()
		{
			digits[pos] = indexList.First((int i) => i > digits[pos] && !digits.Take(pos).Contains(i));
			int j;
			for (j = pos + 1; j <= count - 1; j++)
			{
				digits[j] = indexList.First((int t) => !digits.Take(j).Contains(t));
			}
			SetCurrent();
			pos = -1;
			for (int num = count - 2; num >= 0; num--)
			{
				if (digits[num] < digits[num + 1])
				{
					pos = num;
					break;
				}
			}
			if (pos == -1)
			{
				hasNext = false;
			}
		}
	}
}
