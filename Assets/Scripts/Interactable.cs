using UnityEngine;

namespace Verkefni3
{
    public abstract class Interactable : MonoBehaviour
    {
        public abstract void Interact(GameObject interactor);
    }
}