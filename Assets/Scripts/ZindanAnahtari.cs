using UnityEngine;

public class ZindanAnahtari : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ScoreManager'da anahtar sayısını artır veya bir bool tut
            if (ScoreManager.instance != null)
            {
                // ScoreManager içinde 'public bool anahtarVar = false;' olduğunu varsayıyoruz
                ScoreManager.instance.anahtarVar = true; 
            }

            Debug.Log("Anahtar Alındı! Artık Zindan Kapısını Açabilirsin.");
            
            // Anahtarı sahneden yok et
            Destroy(gameObject);
        }
    }
    void Update() {
    transform.position += new Vector3(0, Mathf.Sin(Time.time * 3f) * 0.0005f, 0);
}
}