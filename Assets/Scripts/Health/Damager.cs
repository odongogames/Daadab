using UnityEngine;

namespace Daadab
{
    public class Damager : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                Debug.Log($"{name} apply damage to {damageable}");
                damageable.TakeDamage();
            }
        }
    }
}
