 using UnityEngine;

public class BossMusicTrigger : MonoBehaviour
{
    [Header("Ses Kaynakları")]
    public AudioSource bossMusic;    // Buraya Boss müziğinin olduğu objeyi sürükle
    public AudioSource mainBackgroundMusic; // Buraya oyunun normal müziğinin olduğu objeyi sürükle

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Sadece Player değdiğinde ve daha önce tetiklenmediyse çalış
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            // 1. Önce normal müziği durdur
            if (mainBackgroundMusic != null)
            {
                mainBackgroundMusic.Stop();
            }

            // 2. Boss müziğini başlat
            if (bossMusic != null)
            {
                bossMusic.Play();
                Debug.Log("BOSS SAVAŞI BAŞLADI: Müzik Değişti!");
            }
        }
    }
}