using UnityEngine;

namespace Daadab
{
    public class DisableOnAwake : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}
