using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class ResetRectTransformPosition : MonoBehaviour
    {
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            Assert.IsNotNull(rectTransform);

            rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
