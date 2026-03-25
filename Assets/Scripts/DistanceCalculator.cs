using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class DistanceCalculator : GameStateSubscriber
    {
        public static DistanceCalculator Instance;

        [SerializeField] private Transform worldStart;
        [SerializeField] private Transform worldEnd;

        [Header("Runtime only")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float worldLength;
        [SerializeField] private float playerDistToWorldEnd;
        [SerializeField][Range(0, 1)] private float playerDistToWorldEndNormalised;

        public float WorldLength => worldLength;
        public float PlayerDistToWorldEndNormalised => playerDistToWorldEndNormalised;

        private SectionSpawner sectionSpawner;

        public override void Awake()
        {
            if (Instance != null)
            {
                Debug.Log($"{GetType()} has been found. Destroying self...");
                Destroy(gameObject);
                return;
            }

            Instance = this;

            base.Awake();

            Assert.IsNotNull(worldStart);
            Assert.IsNotNull(worldEnd);

            Assert.IsTrue(worldEnd.position.z > worldStart.position.z);

            worldLength = worldEnd.position.z - worldStart.position.z;
        }

        private void Start()
        {
            var truck = Truck.Instance;
            Assert.IsNotNull(truck);

            playerTransform = truck.transform;
            Assert.IsNotNull(playerTransform);

            sectionSpawner = SectionSpawner.Instance;
            Assert.IsNotNull(sectionSpawner);
        }

        private void Update()
        {
            playerDistToWorldEnd =
                sectionSpawner.LastSection.SectionEnd.position.z - playerTransform.position.z;

            playerDistToWorldEndNormalised = playerDistToWorldEnd / worldLength;
        }

        public Vector3 GetPositionAheadOfPlayer(float forwardDistance)
        {
            return playerTransform.position + new Vector3(0, 0, forwardDistance);
        }
    }
}
