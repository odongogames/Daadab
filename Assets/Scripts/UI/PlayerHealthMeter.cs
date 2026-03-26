using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class PlayerHealthMeter : MonoBehaviour
    {
        [SerializeField] private HeartContainer[] heartContainers;
        private Health playerHealth;
        private Animator animator;

        private int growHash;

        private void Awake()
        {
            //TODO: put an animator on each heart container
            // animator = GetComponent<Animator>();
            // Assert.IsNotNull(animator);

            Assert.IsTrue(heartContainers.Length == 3);
        }

        private void Start()
        {
            var truck = Truck.Instance;
            Assert.IsNotNull(truck);

            playerHealth = truck.GetComponent<Health>();
            Assert.IsNotNull(playerHealth);

            playerHealth.OnTakeDamage += Player_OnTakeDamage;
            playerHealth.OnAddHealth += Player_OnAddHealth;

            GameManager.OnSetupGame += GameManager_OnSetupGame;
        }

        private void OnDestroy()
        {
            playerHealth.OnTakeDamage -= Player_OnTakeDamage;
            playerHealth.OnAddHealth -= Player_OnAddHealth;

            GameManager.OnSetupGame -= GameManager_OnSetupGame;
        }

        private void GameManager_OnSetupGame()
        {
            for (int i = 0; i < heartContainers.Length; i++)
            {
                heartContainers[i].Activate();
            }
        }

        private void Player_OnTakeDamage(uint value)
        {
            heartContainers[value].Deactivate();
        }
        
        private void Player_OnAddHealth(uint value)
        {
            heartContainers[value - 1].Activate();
        }
    }
}
