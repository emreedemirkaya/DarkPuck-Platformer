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

        if (currentHealth <= 0) Die();
        else StartCoroutine(InvulnerabilityRoutine()); // Hasar alınca ölümsüzlüğü başlat
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

    void Die() { Debug.Log("Oyun Bitti!"); }
}