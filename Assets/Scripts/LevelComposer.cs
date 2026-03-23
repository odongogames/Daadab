using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class LevelComposer : MonoBehaviour
    {
        [SerializeField] private Transform worldStart;
        [SerializeField] private Transform worldEnd;
        [SerializeField] private Transform playerTransform;

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
        [SerializeField] [Range(0,1)] private float sectionCreateDistanceNormalised = 0.8f;
        /// <summary>
        /// The most recently spawned section
        /// </summary>
        [SerializeField] private Section lastSection;
        [SerializeField] private List<Section> sections = new();

        [Header("Runtime Only")]
        [SerializeField] private bool initialised;
        [SerializeField] private float worldLength;
        [SerializeField] private float playerDistToWorldEnd;
        [SerializeField] [Range(0,1)] private float playerDistToWorldEndNormalised;
        

        private int sectionIndex;

        private void Awake()
        {
            Assert.IsNotNull(worldStart);
            Assert.IsNotNull(worldEnd);
            Assert.IsNotNull(sectionTemplate);
            Assert.IsNotNull(sectionHolder);
            Assert.IsNotNull(playerTransform);

            for (int i = 0; i < sectionAmount; i++)
            {
                var obj = Instantiate(sectionTemplate, sectionHolder);
                obj.name = $"Section {i + 1}";
                obj.gameObject.SetActive(false);

                sections.Add(obj);
            }

            Assert.IsTrue(worldEnd.position.z > worldStart.position.z);
            Assert.IsTrue(sectionCreateDistanceNormalised > 0);

            worldLength = worldEnd.position.z - worldStart.position.z;
            Assert.IsTrue(worldLength > 0);

            int counter = 0; 

            while (counter < sectionAmount && spawnedDistance < worldLength)
            {
                counter++;
                ActivateNextSection();
            }

            initialised = true;
        }

        private void Update()
        {
            if (!initialised) return;

            playerDistToWorldEnd =
                lastSection.SectionEnd.position.z - playerTransform.position.z;

            playerDistToWorldEndNormalised = playerDistToWorldEnd / worldLength;

            if (playerDistToWorldEndNormalised < sectionCreateDistanceNormalised)
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
                    z: worldStart.position.z
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
    }
}
