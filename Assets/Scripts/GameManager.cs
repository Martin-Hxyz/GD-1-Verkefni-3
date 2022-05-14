using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Verkefni3
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI announcementText;

        private AudioSource m_AudioSource;
        private int m_Score = 0;

        private void Start()
        {
            instance = this;
            m_AudioSource = GetComponent<AudioSource>();
        }

        public void ChangeScore(int amount)
        {
            m_Score += amount;
            scoreText.text = m_Score.ToString();
        }

        public void PlaySound(AudioClip clip)
        {
            m_AudioSource.PlayOneShot(clip);
        }

        public void ShowText(String text)
        {
            StartCoroutine(DisplayTextFor(text, 3.5f));
        }

        public void LoadStartMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void LoadMainScene()
        {
            SceneManager.LoadScene(1);
        }

        public void LoadGameOver()
        {
            SceneManager.LoadScene(2);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private IEnumerator DisplayTextFor(string health, float seconds)
        {
            announcementText.text = health;
            yield return new WaitForSeconds(seconds);
            announcementText.text = "";
        }
    }
}