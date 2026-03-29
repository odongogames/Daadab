using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class ObjectSpawner : GameStateSubscriber
    {
        public static ObjectSpawner Instance;

        [SerializeField] private GameObject finalObject;
        [SerializeField] private Transform objectHolder;
        // TODO: Consider moving to registry
        /// <summary>
        /// How many object sequences do we spawn in total?
        /// </summary>
        [SerializeField] private uint totalObjectSequenceCount = 12;
        /// <summary>
        /// How far in front of the player do we need to spawn objects?
        /// </summary>
        [SerializeField] private float spawnAheadDistance = 60;

        /// <summary>
        /// At what distance do we begin spawning objects?
        /// </summary>
        [SerializeField] private float initialSpawnDistance = 20;
        [SerializeField] private List<PooledObjectSequence> sourceObjectSequenceList= new();
        [SerializeField] private List<PooledObjectSequence> gameplayObjectSequenceList = new();

        [SerializeField] private int extraObjectCount = 4;
        [SerializeField] private PooledObject[] extraObjectsToSpawn;

        [Header("Runtime Only")]
        [SerializeField] private PooledObjectSequence currentObjectSequence;
        [SerializeField] private List<PooledObjectCount> objectCounts = new();
        [SerializeField] private List<PooledObjectCount> gameplayObjectCounts = new();

        private float desiredSpawnDistance;
        private float totalSpawnDistance;
        private int objectSequenceSpawnCount;

        private GameManager gameManager;
        private Registry registry;
        private ObjectPool objectPool;
        private DistanceCalculator distanceCalculator;

        public override void Awake()
        {
            if (Instance != null)
            {
                Debug.Log($"Destroying {this.GetType()} as more than one instance found.");
                Destroy(this);
                return;
            }

            Instance = this;

            base.Awake();

            registry = Registry.Instance;
            Assert.IsNotNull(registry);

            gameManager = GameManager.Instance;
            Assert.IsNotNull(gameManager);

            distanceCalculator = DistanceCalculator.Instance;
            Assert.IsNotNull(distanceCalculator);

            objectPool = ObjectPool.Instance;
            Assert.IsNotNull(objectPool);

            Assert.IsNotNull(objectHolder);
            Assert.IsNotNull(finalObject);

            Assert.IsTrue(sourceObjectSequenceList.Count > 0);

            objectPool = ObjectPool.Instance;

            CountPooledObjectsFromObjectSequences();

            foreach (var count in objectCounts)
            {
                // var poolAmount = Mathf.CeilToInt(count.Count / objectCounts.Count);
                objectPool.AddPool(count.PooledObject, (int)count.Count);
            }

            foreach (var obj in extraObjectsToSpawn)
            {
                objectPool.AddPool(obj, extraObjectCount);
            }
        }

        public float GetTotalWorldDistance()
        {
            return totalObjectSequenceCount * registry.ObjectSequenceLength + initialSpawnDistance;
        }

        public override void Initialise()
        {
            base.Initialise();

            gameplayObjectSequenceList.Clear();

            for (int i = 0; i < totalObjectSequenceCount; i++)
            {
                gameplayObjectSequenceList.Add(sourceObjectSequenceList[UnityEngine.Random.Range(0, sourceObjectSequenceList.Count)]);
            }

            CountPooledObjectsFromGameplayObjectSequences();

            foreach (var count in gameplayObjectCounts)
            {
                if (string.Equals(count.PooledObject.name, "water", StringComparison.CurrentCultureIgnoreCase))
                {
                    registry.SetTotalWaterCount(count.Count);
                }
            }

            objectSequenceSpawnCount = 0;

            finalObject.transform.position = Vector2.zero;
            finalObject.SetActive(false);
            totalSpawnDistance = distanceCalculator.GetPositionAheadOfPlayer(initialSpawnDistance).z;

            Debug.Log("Initialise object spawner");
        }

        private void Update()
        {
            if (gameStateMachine.GetCurrentState() != GameState.Gameplay) return;

            desiredSpawnDistance = distanceCalculator.GetPositionAheadOfPlayer(spawnAheadDistance).z;

            while (!HasFinishedSpawningObjectSequences() && totalSpawnDistance < desiredSpawnDistance)
            {
                SpawnObjects();
            }
        }

        public void SpawnObjects()
        {
            currentObjectSequence = gameplayObjectSequenceList[objectSequenceSpawnCount];

            SpawnLane(currentObjectSequence.LeftLaneHolder, objectHolder);
            SpawnLane(currentObjectSequence.MidLaneHolder, objectHolder);
            SpawnLane(currentObjectSequence.RightLaneHolder, objectHolder);

            totalSpawnDistance += registry.ObjectSequenceLength;

            objectSequenceSpawnCount++;

            if (HasFinishedSpawningObjectSequences())
            {
                SpawnFinalObject();
            }
        }

        private void SpawnFinalObject()
        {
            Debug.Log("Spawn final object");
            
            var position = Vector3.zero;
            position.z += totalSpawnDistance;
            finalObject.transform.position = position;

            finalObject.SetActive(true);
        }

        private bool HasFinishedSpawningObjectSequences()
        {
            return objectSequenceSpawnCount >= totalObjectSequenceCount;
        }

        private void SpawnLane(Transform source, Transform laneHolder)
        {
            foreach (Transform child in source)
            {
                // var unit = Instantiate(data.UnitVariable.GetValue(), laneHolder);
                var obj = objectPool.GetPooledObject(child.name);
                Assert.IsNotNull(obj);

                // obj.Transform.SetParent(laneHolder);
                var position = child.localPosition + source.localPosition;
                position.z += totalSpawnDistance;
                obj.Transform.localPosition = position;

                obj.Transform.localRotation = child.localRotation;
                obj.SpawnMe?.Invoke();

                obj.GameObject.SetActive(true);
            }
        }

        private void CountPooledObjectsFromObjectSequences()
        {
            foreach (var sequence in sourceObjectSequenceList)
            {
                CountPooledObjectsFromTransform(sequence.LeftLaneHolder, objectCounts);
                CountPooledObjectsFromTransform(sequence.MidLaneHolder, objectCounts);
                CountPooledObjectsFromTransform(sequence.RightLaneHolder, objectCounts);
            }
        }

        private void CountPooledObjectsFromGameplayObjectSequences()
        {
            gameplayObjectCounts.Clear();

            foreach (var sequence in gameplayObjectSequenceList)
            {
                CountPooledObjectsFromTransform(sequence.LeftLaneHolder, gameplayObjectCounts);
                CountPooledObjectsFromTransform(sequence.MidLaneHolder, gameplayObjectCounts);
                CountPooledObjectsFromTransform(sequence.RightLaneHolder, gameplayObjectCounts);
            }
        }

        private void CountPooledObjectsFromTransform(Transform trans, List<PooledObjectCount> countList)
        {
            foreach (Transform child in trans)
            {
                var pooledObject = child.GetComponent<PooledObject>();
                if (null == pooledObject)
                {
                    Debug.LogWarning($"{child.name} is not a pooled object", child);
                    continue;
                }

                bool found = false;

                for (int i = 0; i < countList.Count; i++)
                {
                    if (string.Equals(countList[i].PooledObject.name, pooledObject.name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        countList[i].Count++;
                        found = true;
                    }
                }

                if (!found)
                {
                    countList.Add(new PooledObjectCount
                    {
                        PooledObject = pooledObject,
                        Count = 1
                    });
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            var position = Vector3.zero;

            position.z = totalSpawnDistance;
            Gizmos.color = Color.rebeccaPurple;
            Gizmos.DrawSphere(position, 1);            
            
            // position.z = desiredSpawnDistance;
            // Gizmos.color = Color.gainsboro;
            // Gizmos.DrawSphere(position, 1);
        }
#endif
    }
}
