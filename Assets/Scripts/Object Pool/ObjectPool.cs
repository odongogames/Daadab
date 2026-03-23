// Original source: Unity Technologies

using System.Collections.Generic;
using UnityEngine;

namespace Daadab
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private uint initPoolSize;
        [SerializeField] private PooledObject objectToPool;
        // store the pooled objects in a collection
        private Stack<PooledObject> stack;

        private void Start()
        {
            SetupPool();
        }

        // creates the pool (invoke when the lag is not noticeable)
        private void SetupPool()
        {
            stack = new Stack<PooledObject>();
            
            PooledObject instance = null;
            for (int i = 0; i < initPoolSize; i++)
            {
                instance = Instantiate(objectToPool);
                instance.Pool = this;
                instance.gameObject.SetActive(false);
                stack.Push(instance);
            }
        }
        
        // returns the first active GameObject from the pool
        public PooledObject GetPooledObject()
        {
            // if the pool is not large enough, instantiate a new PooledObjects
            if (stack.Count == 0)
            {
                PooledObject newInstance = Instantiate(objectToPool);
                newInstance.Pool = this;
                return newInstance;
            }

            // otherwise, just grab the next one from the list
            PooledObject nextInstance = stack.Pop();
            nextInstance.gameObject.SetActive(true);

            return nextInstance;
        }
        public void ReturnToPool(PooledObject pooledObject)
        {
            stack.Push(pooledObject);
            pooledObject.gameObject.SetActive(false);
        }
    }
}