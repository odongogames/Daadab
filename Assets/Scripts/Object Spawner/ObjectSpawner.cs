using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class ObjectSpawner : GameStateSubscriber
    {
        [SerializeField] private Registry registry;
        [SerializeField] private Transform objectHolder;
        /// <summary>
        /// How far in front of the player do we need to spawn objects?
        /// </summary>
        [SerializeField] private float spawnAheadDistance = 60;

        /// <summary>
        /// At what distance do we begin spawning objects?
        /// </summary>
        [SerializeField] private float initialSpawnDistance = 20;
        [SerializeField] private PooledObjectSequence[] objectSequences;

        [Header("Runtime Only")]
        [SerializeField] private PooledObjectSequence currentObjectSequence;
        [SerializeField] private List<PooledObject> pooledObjects = new();
        [SerializeField] private List<PooledObjectCount> objectCounts = new();

        private float desiredSpawnDistance;
        private float totalSpawnDistance;
        private float lastSpawnPosition;
        private int objectSequenceSpawnCount;

        private ObjectPool objectPool;
        private DistanceCalculator distanceCalculator;

        public override void Awake()
        {
            base.Awake();

            Assert.IsNotNull(registry);

            distanceCalculator = DistanceCalculator.Instance;
            Assert.IsNotNull(distanceCalculator);

            objectPool = ObjectPool.Instance;
            Assert.IsNotNull(objectPool);

            Assert.IsNotNull(objectHolder);

            Assert.IsTrue(objectSequences.Length > 0);

            objectPool = ObjectPool.Instance;

            CountPooledObjectsFromObjectSequences();

            foreach (var count in objectCounts)
            {
                // var poolAmount = Mathf.CeilToInt(count.Count / objectCounts.Count);
                objectPool.AddPool(count.PooledObject, (int)count.Count);
            }
        }

        public override void Initialise()
        {
            base.Initialise();
            
            totalSpawnDistance = distanceCalculator.GetPositionAheadOfPlayer(initialSpawnDistance).z;
        }

        private void Update()
        {
            desiredSpawnDistance = distanceCalculator.GetPositionAheadOfPlayer(spawnAheadDistance).z;

            while (totalSpawnDistance < desiredSpawnDistance)
            {
                SpawnObjects();
            }
        }

        public void SpawnObjects()
        {
            currentObjectSequence = objectSequences[UnityEngine.Random.Range(0, objectSequences.Length)];

            SpawnLane(currentObjectSequence.LeftLaneHolder, objectHolder);
            SpawnLane(currentObjectSequence.MidLaneHolder, objectHolder);
            SpawnLane(currentObjectSequence.RightLaneHolder, objectHolder);

            totalSpawnDistance += registry.ObjectSequenceLength;
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
                obj.GameObject.SetActive(true);
                pooledObjects.Add(obj);
            }
        }


        private void CountPooledObjectsFromObjectSequences()
        {
            foreach (var sequence in objectSequences)
            {
                CountPooledObjectsFromTransform(sequence.LeftLaneHolder);
                CountPooledObjectsFromTransform(sequence.MidLaneHolder);
                CountPooledObjectsFromTransform(sequence.RightLaneHolder);
            }
        }

        private void CountPooledObjectsFromTransform(Transform trans)
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

                for (int i = 0; i < objectCounts.Count; i++)
                {
                    if (string.Equals(objectCounts[i].PooledObject.name, pooledObject.name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        objectCounts[i].Count++;
                        found = true;
                    }
                }

                if (!found)
                {
                    objectCounts.Add(new PooledObjectCount
                    {
                        PooledObject = pooledObject,
                        Count = 1
                    });
                }
            }
        }

#if UNITY_EDITOR
        private void OrawGizmos()
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
