using System;
using System.Collections;
using System.Collections.Generic;
using Dythervin.Core;

namespace Dythervin.ObjectPool
{
    public static class EnumerableArrayHandlerExt
    {
        /// <summary>
        /// Disposed in its enumerator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static PooledEnumerableArrayHandler<T> ToTempEnumerableArray<T>(this IReadOnlyList<T> list)
        {
            return new PooledEnumerableArrayHandler<T>(list);
        }

        /// <summary>
        /// Disposed in its enumerator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static PooledEnumerableArrayHandler<T> ToTempEnumerableArray<T>(this HashSet<T> list)
        {
            return new PooledEnumerableArrayHandler<T>(list);
        }
    }

    public struct PooledEnumerableArrayHandler<T> : IReadOnlyCollection<T>
    {
        private static readonly bool ClearOnDispose = typeof(T).IsClass;
        private T[] _tempArray;
        private readonly bool _isSystemPool;

        public int Count { get; }

        internal PooledEnumerableArrayHandler(IReadOnlyList<T> list) : this(list.Count)
        {
            for (int i = 0; i < Count; i++)
            {
                _tempArray[i] = list[i];
            }
        }

        public PooledEnumerableArrayHandler(int count)
        {
            Count = count;
            _isSystemPool = ArrayPools.TryGetPool(Count, out IObjectPool<T[]> pool);

            _tempArray = _isSystemPool ? System.Buffers.ArrayPool<T>.Shared.Rent(Count) : pool.Get();
        }

        public PooledEnumerableArrayHandler(HashSet<T> hashSet) : this(hashSet.Count)
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

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct Enumerator : IDisposableExt, IEnumerator<T>
        {
            private PooledEnumerableArrayHandler<T> _self;
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

            public Enumerator(PooledEnumerableArrayHandler<T> self)
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
                this.AssertNotDisposed();
                if (_self._isSystemPool)
                    System.Buffers.ArrayPool<T>.Shared.Return(_self._tempArray, ClearOnDispose);
                else
                    ArrayPools.GetPool<T>(_self.Count).Release(_self._tempArray);

                _self._tempArray = null;
            }

            bool IDisposableExt.IsDisposed => _self._tempArray == null;
        }
    }
}