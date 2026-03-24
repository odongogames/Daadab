using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    // TODO: Move this into Section
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private Registry registry;

        [SerializeField] private Transform leftLaneHolder;
        [SerializeField] private Transform midLaneHolder;
        [SerializeField] private Transform rightLaneHolder;

        [Header("Runtime Only")]
        [SerializeField] private PooledObjectSequence objectSequence;
        [SerializeField] private List<PooledObject> pooledObjects = new();

        private ObjectPool objectPool;

        private void Awake()
        {
            objectPool = ObjectPool.Instance;
            Assert.IsNotNull(objectPool);

            Assert.IsNotNull(registry);
            Assert.IsNotNull(leftLaneHolder);
            Assert.IsNotNull(midLaneHolder);
            Assert.IsNotNull(rightLaneHolder);
        }

        public void Disable()
        {
            foreach (var obj in pooledObjects)
            {
                obj.Release();
            }
            
            pooledObjects.Clear();
        }

        public void SpawnObjects(PooledObjectSequence sequence)
        {
            objectSequence = sequence;

            SpawnLane(sequence.LeftLaneHolder, leftLaneHolder);
            SpawnLane(sequence.MidLaneHolder, midLaneHolder);
            SpawnLane(sequence.RightLaneHolder, rightLaneHolder);
        }

        private void SpawnLane(Transform source, Transform laneHolder)
        {
            foreach (Transform child in source)
            {
                // var unit = Instantiate(data.UnitVariable.GetValue(), laneHolder);
                var obj = objectPool.GetPooledObject(child.name);
                Assert.IsNotNull(obj);
                obj.Transform.SetParent(laneHolder);
                obj.Transform.localPosition = child.localPosition;
                obj.Transform.localRotation = child.localRotation;
                obj.GameObject.SetActive(true);
                pooledObjects.Add(obj);
            }
        }
    }
}
