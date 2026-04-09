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

    // 1. Sahnenin ve UI'ın tam oturması için 1 frame (kare) bekle
    yield return null; 

    // 2. ScoreManager'dan canı çek
    if (ScoreManager.instance != null)
    {
        currentHealth = ScoreManager.instance.mevcutCan;
        Debug.Log("Can Verisi Çekildi: " + currentHealth);
    }
    else
    {
        currentHealth = maxHealth;
    }

    UpdateHealthUI();
}

    public void TakeDamage(float amount)
    {
        if (isInvulnerable) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // ScoreManager'daki kalıcı veriyi anında güncelle
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.mevcutCan = currentHealth;
        }

        UpdateHealthUI();

        if (currentHealth <= 0) Die();
        else StartCoroutine(InvulnerabilityRoutine());
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.mevcutCan = currentHealth;
        }

        UpdateHealthUI();
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
        // Öldüğünde canı sıfırla ki Level 1'den başlarsa fullensin
        if (ScoreManager.instance != null) ScoreManager.instance.mevcutCan = maxHealth;

        GameOverManager gameOverManager = FindFirstObjectByType<GameOverManager>();
        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver(ScoreManager.instance.score);
        }
    }
}