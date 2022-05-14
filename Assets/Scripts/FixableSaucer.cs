using System;
using System.Linq;
using UnityEngine;

namespace Verkefni3
{
    public class FixableSaucer : Interactable
    {
        public GameObject[] neededCollectibles;
        public AudioSource audioSource;
        public AudioClip missingPartsSound;
        public AudioClip fixedSound;

        private bool m_Fixed = false;

        public override void Interact(GameObject interactor)
        {
            if (m_Fixed) return;
            if (!interactor.CompareTag("Player")) return;
            var player = interactor.GetComponent<Player>();

            
            Debug.Log("Interacted with saucer");
            
            if (!CanBeFixed())
            {
                Debug.Log("Saucer cant be fixed");
                GameManager.instance.ShowText("It's out of gas!");
                player.PlaySound(missingPartsSound);
                return;
            }

            Debug.Log("Saucer can be fixed");
            GameManager.instance.ShowText("It's up and running! Let's get out of here");
            player.PlaySound(fixedSound);

            // kallar á StartEngine fallið eftir 1.8s
            Invoke(nameof(StartEngine), 1.8f);

            m_Fixed = true;
        }

        private void StartEngine()
        {
            audioSource.enabled = true;
        }

        // neededCollectibles er listi yfir game objects sem verða að vera 'pikkaðir upp' svo það sé hægt
        // að laga skipið. ef þeir eru allir inactive þá er hægt að laga skipið
        private bool CanBeFixed()
        {
            return neededCollectibles.Count(collectible => collectible.activeInHierarchy) == 0;
        }
    }
}