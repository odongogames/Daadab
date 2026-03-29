using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Daadab
{
    public class MainMenuInput : MonoBehaviour
    {
        [SerializeField] private UIScreen loadingUIScreen;

        private InputReader inputReader;
        private SFXPlayer SFXPlayer;

        private void Awake()
        {
            inputReader = InputReader.Instance;
            Assert.IsNotNull(inputReader);

            SFXPlayer = SFXPlayer.Instance;
            Assert.IsNotNull(SFXPlayer);

            Assert.IsNotNull(loadingUIScreen);
        }

        private void Update()
        {
            if (inputReader.StartEnter())
            {
                SFXPlayer.PlayClickSound();

                loadingUIScreen.Show(instantly: true);
                
                SceneManager.LoadScene("Prototype 1");
            }
        }
    }
}
