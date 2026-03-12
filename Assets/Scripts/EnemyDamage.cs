using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float damageAmount = 20f;
    public float knockbackForceX = 10f;
    public float knockbackForceY = 8f;
    public float knockbackDuration = 0.3f; // Kontrol kaybı süresi

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            PlayerController controller = collision.gameObject.GetComponent<PlayerController>();

            if (health != null) health.TakeDamage(damageAmount);

            if (controller != null)
            {
                // Yönü belirle (Düşman soldaysa sağa fırlat)
                float dir = collision.transform.position.x > transform.position.x ? 1f : -1f;
                Vector2 force = new Vector2(dir * knockbackForceX, knockbackForceY);
                
                controller.ApplyKnockback(force, knockbackDuration);
            }
        }
    }
}