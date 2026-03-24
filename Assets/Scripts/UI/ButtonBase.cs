using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Daadab
{
    public class ButtonBase : MonoBehaviour
    {
        private Button button;

        public virtual void Awake()
        {
            button = GetComponent<Button>();
            Assert.IsNotNull(button);
        }

        public virtual void Start()
        {
            button.onClick.AddListener(() => { OnClick(); } );
        }

        public virtual void OnClick()
        {
        }
    }
}
