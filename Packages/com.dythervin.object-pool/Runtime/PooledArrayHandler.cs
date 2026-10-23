using System;
using System.Collections;
using System.Collections.Generic;
using Dythervin.Core;

namespace Dythervin.ObjectPool
{
    public static class ArrayPoolHandlerExt
    {
        public static PooledArrayHandler<T> ToTempArray<T>(this IReadOnlyList<T> list)
        {
            return new PooledArrayHandler<T>(list);
        }

        public static PooledArrayHandler<T> ToTempArray<T>(this HashSet<T> list)
        {
            return new PooledArrayHandler<T>(list);
        }
    }

    public struct PooledArrayHandler<T> : IDisposableExt, IReadOnlyList<T>
    {
        private static readonly bool ClearOnDispose = typeof(T).IsClass;
        private T[] _tempArray;
        private readonly bool _isSystemPool;

        public int Count { get; }

        public bool IsDisposed { get; private set; }

        public readonly T this[int index]
        {
            get
            {
                AssertIndex(index);
                return _tempArray[index];
            }
            set
            {
                AssertIndex(index);
                _tempArray[index] = value;
            }
        }

        internal PooledArrayHandler(IReadOnlyList<T> list) : this(list.Count)
        {
            for (int i = 0; i < Count; i++)
            {
                _tempArray[i] = list[i];
            }
        }

        public PooledArrayHandler(int count)
        {
            Count = count;
            _isSystemPool = ArrayPools.TryGetPool(Count, out IObjectPool<T[]> pool);

            _tempArray = _isSystemPool ? System.Buffers.ArrayPool<T>.Shared.Rent(Count) : pool.Get();
            IsDisposed = false;
        }

        public PooledArrayHandler(HashSet<T> hashSet) : this(hashSet.Count)
        {
            int i = 0;
            foreach (T value in hashSet)
            {
                _tempArray[i++] = value;
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        private readonly void AssertIndex(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public void Dispose()
        {
            this.AssertNotDisposed();
            if (_isSystemPool)
                System.Buffers.ArrayPool<T>.Shared.Return(_tempArray, ClearOnDispose);
            else
                ArrayPools.GetPool<T>(Count).Release(_tempArray);

            _tempArray = null;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct Enumerator : IEnumerator<T>
        {
            private PooledArrayHandler<T> _self;
            private int _index;

            public T Current
            {
                get
                {
                    if (_index < 0 || _index > _self.Count)
                    {
                        throw new ArgumentOutOfRangeException(nameof(_index));
                    }

                    return _self._tempArray[_index];
                }
            }

            object IEnumerator.Current => Current;

            public Enumerator(PooledArrayHandler<T> self)
            {
                _self = self;
                _index = -1;
            }

            public bool MoveNext()
            {
                _index++;
                return _index < _self.Count;
            }

            public void Reset()
            {
                _index = -1;
            }

            public void Dispose()
            {
                _self = default;
            }
        }
    }
}