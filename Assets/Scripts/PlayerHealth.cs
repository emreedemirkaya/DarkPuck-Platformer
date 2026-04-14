using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Ayarlar")]
    public float maxHealth = 100f;
    public float currentHealth;
    
    [Header("UI Görsel")]
    public Image healthBarFill;
    public Gradient healthGradient;
    public float uiUpdateSpeed = 5f; // Barın dolma hızı
    
    private SpriteRenderer spriteRenderer;
    private bool isInvulnerable = false;
    private float targetFillAmount;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Referans kontrolü (Awake içinde yapmak daha güvenlidir)
        if (healthBarFill == null)
        {
            GameObject barGo = GameObject.Find("health_bar_filler"); 
            if (barGo != null) healthBarFill = barGo.GetComponent<Image>();
        }
    }

    void Start()
    {
        // ScoreManager'dan canı çek veya varsayılanı ata
        if (ScoreManager.instance != null)
        {
            currentHealth = ScoreManager.instance.mevcutCan;
        }
        else
        {
            currentHealth = maxHealth;
        }

        targetFillAmount = currentHealth / maxHealth;
        UpdateUIInstant(); // İlk açılışta bar anında dolsun
    }

    void Update()
    {
        // Can barını pürüzsüz bir şekilde hedef değere ulaştır
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = Mathf.MoveTowards(healthBarFill.fillAmount, targetFillAmount, uiUpdateSpeed * Time.deltaTime);
            healthBarFill.color = healthGradient.Evaluate(healthBarFill.fillAmount);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isInvulnerable || currentHealth <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        SyncWithScoreManager();
        targetFillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0) 
        {
            Die();
        }
        else 
        {
            StartCoroutine(InvulnerabilityRoutine());
        }
    }

    public void Heal(float amount)
    {
        if (currentHealth <= 0) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        SyncWithScoreManager();
        targetFillAmount = currentHealth / maxHealth;
    }

    private void SyncWithScoreManager()
    {
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.mevcutCan = currentHealth;
        }
    }

    private void UpdateUIInstant()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = targetFillAmount;
            healthBarFill.color = healthGradient.Evaluate(targetFillAmount);
        }
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        // 2 saniye boyunca yanıp sön
        float timer = 0;
        while (timer < 2f)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Aç/Kapa
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }
        spriteRenderer.enabled = true; // Sonuçta mutlaka açık kalsın
        isInvulnerable = false;
    }

    void Die()
    {
        // Ölünce canı ScoreManager'da sıfırla veya resetle
        if (ScoreManager.instance != null) ScoreManager.instance.mevcutCan = maxHealth;
        
        GameOverManager gameOverManager = Object.FindFirstObjectByType<GameOverManager>();
        if (gameOverManager != null) 
            gameOverManager.ShowGameOver(ScoreManager.instance != null ? ScoreManager.instance.score : 0);
    }
}