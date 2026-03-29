using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class PooledObject : MonoBehaviour
    {
        [SerializeField] private bool clearAtEndOfBoost;
        private float clearDistanceAroundPlayerAtEndOfBoost = 20;

        private ObjectPool pool;
        private Transform myTransform;
        private Transform playerTransform;
        private GameObject myGameObject;

        public ObjectPool Pool { get => pool; set => pool = value; }

        public Action SpawnMe;

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

            GameManager.OnResetGame += GameManager_OnResetGame;
            SpeedBooster.OnFinishBoost += SpeedBooster_OnFinishBoost;
        }

        private void Start()
        {
            var truck = Truck.Instance;
            Assert.IsNotNull(truck);

            playerTransform = truck.transform;
            Assert.IsNotNull(playerTransform);
        }

        private void OnDestroy()
        {
            SpeedBooster.OnFinishBoost -= SpeedBooster_OnFinishBoost;
            GameManager.OnResetGame -= GameManager_OnResetGame;
        }

        private void OnDisable()
        {
            Release();
        }

        private void GameManager_OnResetGame()
        {
            Release();
        }

        private void SpeedBooster_OnFinishBoost()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            if (!clearAtEndOfBoost) return;
            
            float distanceToPlayer =
                Vector3.Distance(playerTransform.position, myTransform.position);

            if (distanceToPlayer < clearDistanceAroundPlayerAtEndOfBoost)
            {
                Release();
            }
        }

        public void Release()
        {
            pool.ReturnToPool(this);
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
