using UnityEngine;

namespace Core
{
    public class Pickup : MonoBehaviour
    {
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Player"))
                Destroy(gameObject);
        }
    }
}