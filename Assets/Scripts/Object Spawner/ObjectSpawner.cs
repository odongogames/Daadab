using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private Registry registry;
        [SerializeField] private LevelComposer levelComposer;
        [SerializeField] private Transform playerTransform;
        /// <summary>
        /// What distance ahead of the player should be covered by the objects we spawn? 
        /// </summary>
        [SerializeField] private uint desiredSpawnDistance = 100;
        /// <summary>
        /// Don't spawn objects immediately in front of the player when the game starts.
        /// </summary>
        [SerializeField] private uint initialSpawnInterval = 25;
        /// <summary>
        /// Distance along the z-axis between objects we spawn
        /// </summary>
        [SerializeField] private uint spawnInterval = 10;
        [SerializeField] private Transform objectHolder;
        [SerializeField] private List<GameObject> objectPrefabs = new List<GameObject>();

        [Header("Runtime only")]
        [SerializeField] private bool initialised;
        [SerializeField] private float playerDistToObjectsEnd;
        /// <summary>
        /// What the total distance ahead of the player that we have covered with objects? 
        /// </summary>
        [SerializeField] private float totalSpawnDistance;
        /// <summary>
        /// Position on the z-axis where next objects will be spawned
        /// </summary>
        [SerializeField] private float nextSpawnPosition;
        [SerializeField] private float lastSpawnPosition;
        [SerializeField] private uint objectSpawnCounter;

        private void Awake()
        {
            Assert.IsNotNull(levelComposer);
            Assert.IsNotNull(registry);
            Assert.IsTrue(objectPrefabs.Count > 0);
        }

        private void Start()
        {
            int counter = 0;
            int maxCount = 10; //prevent infinite loop

            while (counter < maxCount && totalSpawnDistance < desiredSpawnDistance)
            {
                ActivateNextObjects();
            }

            initialised = true;
        }
        
        private void Update()
        {
            if (!initialised) return;

            playerDistToObjectsEnd =
                lastSpawnPosition - playerTransform.position.z;

            if (playerDistToObjectsEnd < desiredSpawnDistance)
            {
                ActivateNextObjects();
            }
        }

        private void ActivateNextObjects()
        {
            var obj =
            Instantiate(objectPrefabs[Random.Range(0, objectPrefabs.Count)], objectHolder);

            if (objectSpawnCounter == 0)
            {
                nextSpawnPosition = initialSpawnInterval;
            }
            else if (objectSpawnCounter > 0)
            {
                nextSpawnPosition = lastSpawnPosition + spawnInterval;
            }

            totalSpawnDistance += spawnInterval;

            var lane = Random.Range(-1, 1);

            obj.transform.position = new Vector3(
                x: lane * registry.LaneDistance,
                y: 0,
                z: nextSpawnPosition
            );

            objectSpawnCounter++;
            lastSpawnPosition = nextSpawnPosition;
        }
    }
}
