// Dandelion.Maths.CombinationIterator<T>
using Mathy.Utils.Dandelion.Maths;
using System;
using System.Collections;
using System.Collections.Generic;
namespace Mathy.Utils.Dandelion.Maths
{
    public class CombinationIterator<T> : IEnumerable<T[]>, IEnumerable
    {
        private class CombinationIteratorEnumerator<T> : IEnumerator<T[]>, IDisposable, IEnumerator
        {
            private CombinationIterator<T> owner;

            public T[] Current
            {
                get
                {
                    owner.FindNext();
                    return owner.current;
                }
            }

            object IEnumerator.Current => Current;

            public CombinationIteratorEnumerator(CombinationIterator<T> owner)
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

        private int sampleCount;

        private int[] digits;

        private T[] current;

        private bool hasNext;

        public CombinationIterator(T[] source, int sampleCount)
        {
            this.source = source;
            this.sampleCount = sampleCount;
            digits = new int[sampleCount];
            current = new T[sampleCount];
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
            for (int i = 0; i <= sampleCount - 1; i++)
            {
                digits[i] = i;
            }
            digits[sampleCount - 1]--;
        }

        private void FindNext()
        {
            int num = sampleCount - 1;
            while (num >= 0 && digits[num] == source.Length - (sampleCount - num))
            {
                num--;
            }
            digits[num]++;
            for (int i = num + 1; i <= sampleCount - 1; i++)
            {
                digits[i] = digits[i - 1] + 1;
            }
            if ((num == 0 && digits[num] == source.Length - sampleCount) || source.Length == sampleCount)
            {
                hasNext = false;
            }
            for (int i = 0; i <= sampleCount - 1; i++)
            {
                current[i] = source[digits[i]];
            }
        }
    }
}
