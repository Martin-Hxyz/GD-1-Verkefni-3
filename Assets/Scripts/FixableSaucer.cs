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
            // hunsa allt interaction ef búið er að laga 
            if (m_Fixed) return;

            // bara playerinn má nota þetta. frekar tilgangslaust í svona einföldum leik en t.d. gætu karakterar í heiminum alveg eins notað þetta kerfi.
            if (!interactor.CompareTag("Player")) return;
            var player = interactor.GetComponent<Player>();


            if (!CanBeFixed())
            {
                GameManager.instance.ShowText("It's out of gas!");
                player.PlaySound(missingPartsSound);
                return;
            }

            GameManager.instance.ShowText("It's up and running! Let's get out of here");
            player.PlaySound(fixedSound);

            // kallar á StartEngine fallið eftir 1.8s
            Invoke(nameof(StartEngine), 1.8f);

            m_Fixed = true;
        }

        private void StartEngine()
        {
            // kveikir á audio sourcinu, sem spilar vélarhljóð
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