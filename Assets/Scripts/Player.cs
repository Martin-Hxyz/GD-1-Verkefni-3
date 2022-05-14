using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Verkefni3
{
    public class Player : MonoBehaviour
    {
        public TextMeshProUGUI textBox;
        public TextMeshProUGUI scoreBox;
        public GameObject bulletPrefab;
        public AudioClip deathSound;
        public Mask healthBarMask;
        private float healthBarMaxSize;


        private int m_Health;
        private readonly int m_MaxHealth = 100;
        private bool m_Alive = true;
        private AudioSource m_AudioSource;


        private void Start()
        {
            m_Health = m_MaxHealth;
            m_AudioSource = GetComponent<AudioSource>();
            healthBarMaxSize = healthBarMask.rectTransform.rect.width;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                HandleInteract();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                HandleShot();
            }
        }

        public int GetHealth()
        {
            return m_Health;
        }

        public void ChangeHealth(int amount)
        {
            // Þessi kóði á aldrei að keyra ef playerinn er ekki lifandi
            if (!m_Alive) return;

            m_Health = Math.Clamp(m_Health + amount, 0, m_MaxHealth);

            if (m_Health <= 0)
            {
                Die();
            }

            float hpPercent = (float)m_Health / m_MaxHealth;
            healthBarMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                healthBarMaxSize * hpPercent);
        }

        public void PlaySound(AudioClip audioClip)
        {
            m_AudioSource.PlayOneShot(audioClip);
        }

        private void HandleInteract()
        {
            Vector3 rayOrigin = transform.position + (Vector3.up * 1.2f);
            RaycastHit raycastHit;

            // raycast til að gá hvort það sé eih fyrir framan playerinn, eins og við gerðum í rubys adventure
            if (Physics.Raycast(rayOrigin, transform.forward, out raycastHit, 50))
            {
                // interactable er abstract klasi sem ég nota fyrir allskonar MonoBehavior sem vilja leyfa playerinum að ´nota´ þá.
                var interactable = raycastHit.collider.gameObject.GetComponent<Interactable>();
                if (interactable == null)
                {
                    return;
                }

                interactable.Interact(gameObject);
            }
        }

        private void HandleShot()
        {
            var bulletOrigin = transform.position + transform.forward;
            bulletOrigin.y += 1.5f;
            var bullet = Instantiate(bulletPrefab, bulletOrigin, Quaternion.identity);
            var rigibody = bullet.GetComponent<Rigidbody>();
            rigibody.AddForce(transform.forward * 40, ForceMode.Impulse);
        }

        private void Die()
        {
            // Þessi kóði á aldrei að keyra ef playerinn er ekki lifandi
            if (!m_Alive) return;
            GameManager.instance.PlaySound(deathSound);
            Destroy(gameObject);
        }
    }
}