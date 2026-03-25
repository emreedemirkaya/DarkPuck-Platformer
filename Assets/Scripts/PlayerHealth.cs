using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Image healthBarFill;
    public Gradient healthGradient;

    private bool isInvulnerable = false; // Ölümsüzlük kontrolü
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        if (isInvulnerable) return; // Ölümsüzse hasar alma

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); 
        UpdateHealthUI();

        if (currentHealth <= 0) 
        {
            Die();
        }
        else 
        {
            StartCoroutine(InvulnerabilityRoutine()); // Hasar alınca ölümsüzlüğü başlat
        }
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        
        // 2 saniye boyunca yanıp sönme efekti
        for (float i = 0; i < 2f; i += 0.2f)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        isInvulnerable = false;
    }

    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            float fillRatio = currentHealth / maxHealth;
            healthBarFill.fillAmount = fillRatio;
            healthBarFill.color = healthGradient.Evaluate(fillRatio);
        }
    }

    // --- YENİ EKLENEN KISIM: İyileşme Fonksiyonu ---
    public void Heal(float amount)
    {
        // Eğer can zaten tamamsa hiçbir şey yapma
        if (currentHealth >= maxHealth) return; 

        currentHealth += amount;
        
        // Mathf.Clamp sayesinde can 100'ü (maxHealth) asla geçemez. 
        // Yani 85 canı varken 20 eklenirse 105 olmaz, 100'de kalır.
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); 
        
        UpdateHealthUI(); // Can barı görselini güncelle
        Debug.Log("Karakter iyileşti! Yeni can: " + currentHealth);
    }
    void Die() 
    { 
        Debug.Log("Sistem Çöktü! Karakter Öldü."); 

        // 1. Sahnede GameManager'ın içindeki GameOverManager scriptini buluyoruz
        GameOverManager gameOverManager = FindFirstObjectByType<GameOverManager>();
        
        if (gameOverManager != null)
        {
            // 2. ScoreManager'daki Singleton yapısını kullanarak güncel skoru anında çekiyoruz!
            int finalScore = ScoreManager.instance.score; 
            
            // 3. Skoru Game Over paneline gönderip zamanı donduruyoruz.
            gameOverManager.ShowGameOver(finalScore);
        }
        else
        {
            Debug.LogError("Sahnede GameOverManager bulunamadı! Canvas veya GameManager objesine eklendiğinden emin ol.");
        }
    }
}