using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class MusicPlayer : MonoBehaviour
    {
        public static MusicPlayer Instance;

        [SerializeField] private AudioSource audioSource;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log($"Destroying {this.GetType()} as more than one instance found.");
                Destroy(gameObject);
                return;
            }

            Instance = this;

            Assert.IsNotNull(audioSource);

            audioSource.Play();

            DontDestroyOnLoad(gameObject);
        }
    }
}
