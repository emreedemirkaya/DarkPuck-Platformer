using UnityEngine;

public class DotController : MonoBehaviour
{
    public int scoreValue = 10; 
    public AudioClip collectionSound; 
    private AudioSource audioSource;
    private bool isCollected = false; // Çift toplamayı engellemek için

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Eğer objede AudioSource yoksa otomatik ekle
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            CollectDot();
        }
    }

    void CollectDot()
    {
        isCollected = true; // Flag'i true yap ki aynı anda iki kez tetiklenmesin

        // Puan ekleme
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue);
        }

        if (collectionSound != null && audioSource != null)
        {
            // Sesi çal
            audioSource.PlayOneShot(collectionSound);

            // Görseli ve fiziksel etkileşimi hemen kapat
            GetComponent<Collider2D>().enabled = false;
            if (GetComponent<SpriteRenderer>() != null) 
                GetComponent<SpriteRenderer>().enabled = false;

            // Ses dosyasının uzunluğu kadar bekle ve sonra yok et
            // Böylece mp3 dosyan kaç saniyeyse o kadar bekler
            Destroy(gameObject, collectionSound.length); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
}