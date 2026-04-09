using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Ayarlar")]
    public float maxHealth = 100f;
    public float currentHealth;
    
    [Header("Görsel")]
    public Image healthBarFill;
    public Gradient healthGradient;
    
    private SpriteRenderer spriteRenderer;
    private bool isInvulnerable = false;

    IEnumerator Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 1. Sahnenin ve UI sisteminin uyanması için kısa bir süre bekle
        yield return new WaitForSeconds(0.01f); 

        // 2. REFERANS KONTROLÜ: Eğer referans kopmuşsa sahnedeki barı bul
        if (healthBarFill == null)
        {
            // Sahnedeki 'HealthBarFill' isimli objeyi bulmaya çalışır
            GameObject barGo = GameObject.Find("health_bar_filler"); 
            if (barGo != null) healthBarFill = barGo.GetComponent<Image>();
        }

        // 3. ScoreManager'dan canı çek
        if (ScoreManager.instance != null)
        {
            currentHealth = ScoreManager.instance.mevcutCan;
        }
        else
        {
            currentHealth = maxHealth;
        }

        // 4. UI'ı zorla güncelle
        UpdateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        if (isInvulnerable) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.mevcutCan = currentHealth;
        }

        UpdateHealthUI();

        if (currentHealth <= 0) Die();
        else StartCoroutine(InvulnerabilityRoutine());
    }

    // UI Güncelleme Fonksiyonu
    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            // Image Type'ın 'Filled' olduğundan emin olmalısın
            float fillRatio = currentHealth / maxHealth;
            healthBarFill.fillAmount = fillRatio;
            healthBarFill.color = healthGradient.Evaluate(fillRatio);
        }
        else
        {
            Debug.LogWarning("HealthBarFill referansı bulunamadı! Lütfen Inspector'dan atayın.");
        }
    }

    // Diğer fonksiyonlar (Heal, Die, Invulnerability) aynı kalabilir...
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (ScoreManager.instance != null) ScoreManager.instance.mevcutCan = currentHealth;
        UpdateHealthUI();
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        for (float i = 0; i < 2f; i += 0.2f)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        isInvulnerable = false;
    }

    void Die()
    {
        if (ScoreManager.instance != null) ScoreManager.instance.mevcutCan = maxHealth;
        GameOverManager gameOverManager = FindFirstObjectByType<GameOverManager>();
        if (gameOverManager != null) gameOverManager.ShowGameOver(ScoreManager.instance.score);
    }
}