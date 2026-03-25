using UnityEngine;
using UnityEngine.Assertions;

namespace Daadab
{
    public class HeartContainer : MonoBehaviour
    {
        [SerializeField] private GameObject heartInner;
        private Animator animator;

        private int throbHash;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            Assert.IsNotNull(animator);

            throbHash = Animator.StringToHash("throb");
        }

        public void Throb()
        {
            animator.SetTrigger(throbHash);
        }

        public void Activate()
        {
            heartInner.SetActive(true);
            Throb();
        }
        
        public void Deactivate()
        {
            heartInner.SetActive(false);
            Throb();
        }

    }
}
