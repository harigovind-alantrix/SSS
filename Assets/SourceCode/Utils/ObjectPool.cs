using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Utils
{
    public class ObjectPool<T> where T : Component
    {
        private readonly T _prefab;
        private readonly IObjectResolver _container;
        private readonly Transform _poolRoot;
        private readonly Stack<T> _pool = new();
        private readonly int _maxSize;

        public ObjectPool(
            IObjectResolver container,
            T prefab,
            Transform parent,
            int initialSize,
            string poolName,
            int maxSize = 100)
        {
            _container = container;
            _prefab = prefab;
            _maxSize = maxSize;

            var rootGo = new GameObject(poolName);
            _poolRoot = rootGo.transform;
            if (parent != null && parent.gameObject.scene.IsValid())
                _poolRoot.SetParent(parent);

            for (int i = 0; i < initialSize; i++)
            {
                var instance = Instantiate();
                instance.gameObject.SetActive(false);
                _pool.Push(instance);
            }
        }

        public T Get()
        {
            var instance = _pool.Count > 0 ? _pool.Pop() : Instantiate();
            instance.gameObject.SetActive(true);
            return instance;
        }

        public void Return(T instance)
        {
            if (_pool.Count >= _maxSize)
            {
                Object.Destroy(instance.gameObject);
                return;
            }

            instance.gameObject.SetActive(false);
            _pool.Push(instance);
        }

        public void Dispose()
        {
            while (_pool.Count > 0)
            {
                var instance = _pool.Pop();
                if (instance != null)
                    Object.Destroy(instance.gameObject);
            }

            if (_poolRoot != null)
                Object.Destroy(_poolRoot.gameObject);
        }

        private T Instantiate() => _container.Instantiate(_prefab, _poolRoot);
    }
}