using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class VersionText : MonoBehaviour
    {
        private TextMeshProUGUI text;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
            Assert.IsNotNull(text);
            
            text.text = $"v{Application.version}\nOdongo Games";
        }
    }

}
