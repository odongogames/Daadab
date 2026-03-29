using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class PickupSpawner : MonoBehaviour
    {
        static int pickupCount;

        [SerializeField] private PooledObject heartPickup;
        [SerializeField] private PooledObject boostPickup;

        [SerializeField][Range(0, 1)] private float spawnChance = 1;

        private Transform myTransform;

        private ObjectPool objectPool;
        private GameStateMachine gameStateMachine;

        private PooledObject myPooledObject;

        private void Awake()
        {
            Assert.IsNotNull(heartPickup);
            Assert.IsNotNull(boostPickup);

            objectPool = ObjectPool.Instance;
            Assert.IsNotNull(objectPool);

            gameStateMachine = GameStateMachine.Instance;
            Assert.IsNotNull(gameStateMachine);

            myPooledObject = GetComponent<PooledObject>();
            Assert.IsNotNull(myPooledObject);

            myPooledObject.SpawnMe += SpawnObject;

            myTransform = transform;

            GameManager.OnSetupGame += GameManager_OnSetupGame;
        }

        private void OnDestroy()
        {
            myPooledObject.SpawnMe -= SpawnObject;

            GameManager.OnSetupGame -= GameManager_OnSetupGame;
        }

        private void GameManager_OnSetupGame()
        {
            pickupCount = 0;
        }

        // private void OnEnable()
        // {
        //     if (gameStateMachine.GetCurrentState() != GameState.Gameplay) return;

            

        //     // Debug.Log($"Oickup Count: {pickupCount}", this);
        // }

        public void SpawnObject()
        {
            pickupCount++;

            if (Random.value > spawnChance)
            {
                return;
            }

            var objName = "";

            if (pickupCount % 2 == 0)
                objName = heartPickup.name;
            else
                objName = boostPickup.name;

            var obj = objectPool.GetPooledObject(objName);

            obj.Transform.position = myTransform.parent.position + myTransform.localPosition;
            obj.GameObject.SetActive(true);

            Debug.Log($"Spawn pickup: {obj.name}", obj);
        }


    }
}
