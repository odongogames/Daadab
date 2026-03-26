using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class SectionSpawner : GameStateSubscriber
    {
        public static SectionSpawner Instance;

        [Header("Sections")]
        /// <summary>
        /// What distance has been covered along the z-axis by spawning sections?
        /// </summary>
        [SerializeField] private float spawnedDistance;
        [SerializeField] private uint sectionSpawnCounter;
        [SerializeField] private Transform sectionHolder;
        [SerializeField] private Section sectionTemplate;
        /// <summary>
        /// How many sections need to be instantiated
        /// </summary>
        [SerializeField] private uint sectionAmount = 5;
        /// <summary>
        /// Activate next section when playerDistToWorldEndNormalised is less than this value
        /// Example: When player has covered 20% of world playerDistToWorldEndNormalised is 0.8f
        /// if  sectionCreateDistanceNormalised is greater than 0.8f activate next section
        /// </summary>
        [SerializeField][Range(0, 1)] private float sectionCreateDistanceNormalised = 0.8f;
        /// <summary>
        /// The most recently spawned section
        /// </summary>
        [SerializeField] private Section lastSection;
        [SerializeField] private List<Section> sections = new();

        [Header("Runtime Only")]
        private DistanceCalculator distanceCalculator;
        public Section LastSection => lastSection;

        private int sectionIndex;

        private GameManager gameManager;

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


            gameManager = GameManager.Instance;
            Assert.IsNotNull(gameManager);

            distanceCalculator = DistanceCalculator.Instance;
            Assert.IsNotNull(distanceCalculator);

            Assert.IsNotNull(sectionTemplate);
            Assert.IsNotNull(sectionHolder);

            for (int i = 0; i < sectionAmount; i++)
            {
                var obj = Instantiate(sectionTemplate, sectionHolder);
                obj.name = $"Section {i + 1}";
                obj.gameObject.SetActive(false);

                sections.Add(obj);
            }

            Assert.IsTrue(sectionCreateDistanceNormalised > 0);
        }

        public override void Initialise()
        {
            base.Initialise();

            int counter = 0;

            while (counter < sectionAmount && spawnedDistance < distanceCalculator.WorldLength)
            {
                counter++;
                ActivateNextSection();
            }
        }

        private void Update()
        {
            if (!initialised) return;

            if (distanceCalculator.PlayerDistToWorldEndNormalised < sectionCreateDistanceNormalised)
            {
                // Debug.Log($"Player dist: {playerDistToWorldEndNormalised}");
                ActivateNextSection();
            }
        }

        private void ActivateNextSection()
        {
            var section = sections[sectionIndex];

            // Debug.Log($"Next section: {section.name}", section);

            if (sectionSpawnCounter == 0)
            {
                section.transform.position = new Vector3(
                    x: 0,
                    y: 0,
                    z: 0
                );
            }
            else if (sectionSpawnCounter > 0)
            {
                section.transform.position = new Vector3(
                    x: 0,
                    y: 0,
                    z: lastSection.transform.position.z + lastSection.GetLength()
                );
            }

            spawnedDistance += section.GetLength();
            // Debug.Log($"Current level distance: {currentLevelDistance}");

            lastSection = section;
            section.gameObject.SetActive(true);

            sectionSpawnCounter++;
            sectionIndex++;

            if (sectionIndex >= sections.Count)
                sectionIndex = 0;
        }

        public override void ResetMe()
        {
            base.ResetMe();
            
            spawnedDistance = 0;
            sectionSpawnCounter = 0;
            sectionIndex = 0;

            foreach (var section in sections)
            {
                section.gameObject.SetActive(false);
            }

            Debug.Log("Reset section spawner");
        }
    }
}