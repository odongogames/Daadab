using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class FinishLine : MonoBehaviour
    {
        [SerializeField] private AudioClips cheerSound;

        private SFXPlayer SFXPlayer;
        private GameManager gameManager;

        private void Awake()
        {
            Assert.IsNotNull(cheerSound);
            
            gameManager = GameManager.Instance;
            Assert.IsNotNull(gameManager);

            SFXPlayer = SFXPlayer.Instance;
            Assert.IsNotNull(SFXPlayer);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Truck"))
            {
                SFXPlayer.PlayClip(cheerSound);
                gameManager.FinishGame();
            }
        }
    }
}
