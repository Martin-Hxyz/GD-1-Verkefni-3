using System;
using UnityEngine;

namespace Verkefni3
{
    public class Bullet : MonoBehaviour
    {
        public AudioClip killSound;
        private float m_LifeTime;

        private void Update()
        {
            // lifetime check destroyar bulletið 10 sek eftir að það spawnar inn
            
            m_LifeTime += Time.deltaTime;

            if (m_LifeTime >= 10)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // viljum bara drepa enemies
            if (!collision.gameObject.CompareTag("Enemy")) return;
            var enemy = collision.gameObject.GetComponent<Enemy>();
            
            // drepa enemy
            enemy.ChangeHealth(-999);
            
            // bæta við 10 score
            GameManager.instance.ChangeScore(10);

            // ef það er kill sound stillt, spila það
            if (killSound != null)
            {
                GameManager.instance.PlaySound(killSound);
            }

            // eyða bulletinu
            Destroy(gameObject);
        }
    }
}