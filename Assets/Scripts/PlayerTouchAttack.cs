using UnityEngine;

public class PlayerTouchAttack : MonoBehaviour
{
    [Header("Saldırı Ayarları")]
    public int attackDamage = 1; // Drone'a verilecek hasar
    public float bounceForce = 7f; // Çarptıktan sonra oyuncuyu hafifçe geriye itme gücü

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Çarptığımız obje "Enemy" etiketine sahip mi?
        if (collision.gameObject.CompareTag("Enemy"))
        {
            DroneHealth droneHealth = collision.gameObject.GetComponent<DroneHealth>();
            
            if (droneHealth != null)
            {
                // 1. Drone'a hasar ver (DroneHealth scriptindeki TakeDamage çalışır)
                droneHealth.TakeDamage(attackDamage);

                // 2. Oyuncuyu drone'dan hafifçe uzağa doğru sektir (İç içe girmemeleri için)
                if (rb != null)
                {
                    // Oyuncu ile drone arasındaki yönü bul
                    Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
                    
                    // Oyuncunun mevcut hızını sıfırla ve yeni yöne doğru it
                    rb.linearVelocity = Vector2.zero; 
                    rb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}