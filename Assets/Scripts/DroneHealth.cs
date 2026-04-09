using UnityEngine;
using System.Collections;

public class DroneHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;
    private bool isDead = false;

    private Rigidbody2D rb;
    private DroneMovement movementScript;
    private DroneAI aiScript;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        movementScript = GetComponent<DroneMovement>();
        aiScript = GetComponent<DroneAI>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("Drone hasar aldı! Kalan can: " + currentHealth);

        // Vurulduğunda kırmızı yanıp sönme efekti
        StartCoroutine(HitFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        if (isDead) return; 
        isDead = true;

        // --- SKOR EKLEME KISMI ---
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(50);
            Debug.Log("Drone yok edildi, 50 puan eklendi!");
        }

        //OYUNCUYA CAN EKLEME ---
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(20f); // Oyuncuya 20 can ekle
            }
        }

        // Drone'un devriye ve ateş etme zekasını kapat
        if (movementScript != null) movementScript.enabled = false;
        if (aiScript != null) aiScript.enabled = false;

        // SADECE PLAYER İLE ÇARPIŞMAYI KAPAT
        if (player != null)
        {
            Collider2D[] droneColliders = GetComponents<Collider2D>();
            Collider2D[] playerColliders = player.GetComponents<Collider2D>();

            foreach (Collider2D dCol in droneColliders)
            {
                foreach (Collider2D pCol in playerColliders)
                {
                    Physics2D.IgnoreCollision(dCol, pCol, true);
                }
            }
        }
        
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f; 
        rb.constraints = RigidbodyConstraints2D.None; // Dönüş kilidini aç
        rb.AddTorque(15f); // Düşerken takla atsın

        // 0.8 saniye sonra sahneden tamamen sil
        Destroy(gameObject, 0.8f); 
    }

    private IEnumerator HitFlash()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;
        sr.color = Color.red; 
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor; 
    }
}