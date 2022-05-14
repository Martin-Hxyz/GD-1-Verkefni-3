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
            m_LifeTime += Time.deltaTime;

            if (m_LifeTime >= 10)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Enemy")) return;
            var enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.ChangeHealth(-999);
            GameManager.instance.ChangeScore(10);

            if (killSound != null)
            {
                GameManager.instance.PlaySound(killSound);
            }

            Destroy(gameObject);
        }
    }
}