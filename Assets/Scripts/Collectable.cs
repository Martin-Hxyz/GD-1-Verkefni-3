using System;
using UnityEngine;

namespace Verkefni3
{
    public class Collectable : MonoBehaviour
    {
        public bool rotate;
        public AudioClip pickupSound;
        public String pickupText;
        private Transform m_Transform;

        private void Start()
        {
            m_Transform = transform;
        }

        private void Update()
        {
            if (rotate)
            {
                m_Transform.Rotate(0, 90 * Time.deltaTime, 0);
            }
        }

        // disablear game objectinu ef playerinn labbar á það
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            GameManager.instance.PlaySound(pickupSound);
            GameManager.instance.ShowText(pickupText);
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}