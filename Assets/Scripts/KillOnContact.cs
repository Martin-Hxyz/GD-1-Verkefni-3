using UnityEngine;

namespace Verkefni3
{
    public class KillOnContact : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // ekki halda áfram ef það er eih annað en playerinn að snerta
            if (!other.CompareTag("Player")) return;
            var player = other.GetComponent<Player>();
            
            // drepur playerinn
            player.ChangeHealth(-1 * player.GetHealth());
        }
    }
}