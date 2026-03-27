using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class TimeLeftDisplay : GameStateSubscriber
    {
        [SerializeField] private TextMeshProUGUI text;

        private GameManager gameManager;

        public override void Awake()
        {
            base.Awake();

            Assert.IsNotNull(text);

            gameManager = GameManager.Instance;
            Assert.IsNotNull(gameManager);
        }

        private void Update()
        {
            text.text = gameManager.GetTimeLeft().ToString("00");
        }
    }
}
