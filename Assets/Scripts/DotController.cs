using UnityEngine;

public class DotController : MonoBehaviour
{
    public int scoreValue = 10; // Bu yemi toplayınca kaç puan verecek?
    public AudioClip collectionSound; // (İsteğe bağlı) Toplama sesi
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (collectionSound != null && audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eğer yeme "Player" etiketli bir obje çarparsa
        if (other.CompareTag("Player"))
        {
            CollectDot();
        }
    }

    void CollectDot()
    {
        // ScoreManager'a ulaşıp puanı ekle (scoreValue senin yeme verdiğin 10 değeri)
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue);
        }

        // (İsteğe bağlı) Ses oynat
        if (collectionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(collectionSound);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 0.5f); 
        }
        else
        {
            // Ses yoksa hemen yok et
            Destroy(gameObject);
        }
    }
}