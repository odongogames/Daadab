using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class PlayerHealthMeter : MonoBehaviour
    {
        [SerializeField] private GameObject[] heartContainers;
        private Health playerHealth;

        private void Awake()
        {
            Assert.IsTrue(heartContainers.Length == 3);
        }

        private void Start()
        {
            var truck = Truck.Instance;
            Assert.IsNotNull(truck);

            playerHealth = truck.GetComponent<Health>();
            Assert.IsNotNull(playerHealth);

            playerHealth.OnChangeHealth += PlayerHealth_OnChangeHealth;
        }

        private void OnDestroy()
        {
            playerHealth.OnChangeHealth -= PlayerHealth_OnChangeHealth;
        }

        private void PlayerHealth_OnChangeHealth(uint value)
        {
            for (int i = 0; i < heartContainers.Length; i++)
            {
                heartContainers[i].SetActive(i + 1 <= value);
            }
        }
    }
}
