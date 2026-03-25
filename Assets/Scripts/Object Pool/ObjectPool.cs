// Original source: Unity Technologies

using System.Collections.Generic;
using UnityEngine;

namespace Daadab
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance;

        [SerializeField] private uint initPoolSize;

        private Dictionary<string, Stack<PooledObject>> pools = new();
        private Dictionary<string, PooledObject> objectsToPool = new();

        private Transform myTransform;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            myTransform = transform;
        }

        // creates the pool (invoke when the lag is not noticeable)
        public void AddPool(PooledObject pooledObject, int poolSize)
        {
            if (!pools.ContainsKey(pooledObject.name))
            {
                var stack = new Stack<PooledObject>();

                for (int i = 0; i < poolSize; i++)
                {
                    var instance = Instantiate(pooledObject, myTransform);

                    instance.name = pooledObject.name;
                    instance.Pool = this;
                    instance.gameObject.SetActive(false);
                    stack.Push(instance);

                    if(!objectsToPool.ContainsKey(pooledObject.name))
                        objectsToPool.Add(pooledObject.name, instance);
                }

                pools.Add(key: pooledObject.name, value: stack);
            }
        }

        // returns the first active GameObject from the pool
        public PooledObject GetPooledObject(string objectName)
        {
            // Debug.Log($"Try get {objectName} from pool");

            if (pools.TryGetValue(objectName, out Stack<PooledObject> stack))
            {
                // if the pool is not large enough, instantiate a new PooledObjects
                if (stack.Count == 0)
                {
                    if (objectsToPool.TryGetValue(objectName, out PooledObject obj))
                    {
                        PooledObject newInstance = Instantiate(obj);
                        newInstance.Pool = this;
                        newInstance.name = objectName;
                        Debug.Log($"Instantiate new {newInstance.name}", newInstance);
                        return newInstance;
                    }
                }

                // otherwise, just grab the next one from the list
                PooledObject nextInstance = stack.Pop();
                nextInstance.gameObject.SetActive(true);

                // Debug.Log($"Get {nextInstance.name} from pool");

                return nextInstance;
            }

            return null;
        }

        public void ReturnToPool(PooledObject pooledObject)
        {
            if (pools.TryGetValue(pooledObject.name, out Stack<PooledObject> stack))
            {
                stack.Push(pooledObject);
                pooledObject.gameObject.SetActive(false);
                // Debug.Log($"Return {pooledObject.name} to pool");
            }
            else
            {
                Debug.LogWarning($"{pooledObject.name} cannot be returned to pool", pooledObject);
            }
        }
    }
}