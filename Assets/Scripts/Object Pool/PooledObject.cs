using System;
using UnityEngine;

namespace Daadab
{
    public class PooledObject : MonoBehaviour
    {
        [SerializeField] private PooledObjectData data;

        private ObjectPool pool;
        private Transform originalParent;
        private Transform myTransform;
        private GameObject myGameObject;

        public ObjectPool Pool { get => pool; set => pool = value; }
        public PooledObjectData GetData() => data;
        public Transform Transform
        {
            get
            {
                if (null == myTransform)
                {
                    myTransform = transform;
                }

                return myTransform;
            }
        }
        public GameObject GameObject
        {
            get
            {
                if (null == myGameObject)
                {
                    myGameObject = gameObject;
                }

                return myGameObject;
            }
        }

        private void Awake()
        {
            

            myTransform = transform;

            originalParent = myTransform.parent;

            GameManager.OnResetGame += GameManager_OnResetGame;
        }

        private void OnDestroy()
        {
            GameManager.OnResetGame -= GameManager_OnResetGame;
        }

        private void GameManager_OnResetGame()
        {
            Release();
        }

        public void Release()
        {
            pool.ReturnToPool(this);

            // myTransform.SetParent(originalParent, worldPositionStays: false);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PooledObjectRemover"))
            {
                Release();
            }
        }
    }
}
