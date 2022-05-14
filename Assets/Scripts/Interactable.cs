using UnityEngine;

namespace Verkefni3
{
    public abstract class Interactable : MonoBehaviour
    {
        // Abstract klasi sem gerir mjög auðvelt að bæta við hluti sem playerinn getur notað (með E takkanum í þessu tilfelli) t.d. geimskipið
        public abstract void Interact(GameObject interactor);
    }
}