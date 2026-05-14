using UnityEngine;

public class BossMinion : MonoBehaviour
{
    [Header("Ayarlar")]
    public float hiz = 5f;
    public float hasarMiktari = 10f;
    public float omur = 5f;

    private Transform player;

    void Start()
    {
        // Sahnedeki Player'ı bul
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        // Ekranda kalıp şişkinlik yapmasın diye 5 saniye sonra otomatik sil
        Destroy(gameObject, omur);
    }

    void Update()
    {
        if (player != null)
        {
            // Oyuncuya doğru hareket
            Vector2 yon = (player.position - transform.position).normalized;
            transform.Translate(yon * hiz * Time.deltaTime);
        }
    }

    // Durum 1: Eğer Collider "Is Trigger" olarak işaretliyse burası çalışır
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CarpismaGerceklesti(collision.gameObject);
        }
    }

    // Durum 2: Eğer Collider "Is Trigger" DEĞİLSE burası çalışır
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CarpismaGerceklesti(collision.gameObject);
        }
    }

    void CarpismaGerceklesti(GameObject playerObj)
    {
        // Oyuncuya hasar ver
        PlayerHealth ph = playerObj.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(hasarMiktari);
        }

        Debug.Log("Yavru oyuncuya çarptı ve yok ediliyor!");
        
        // YAVRUYU YOK ET
        Destroy(gameObject); 
    }
}