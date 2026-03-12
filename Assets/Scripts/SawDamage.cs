using UnityEngine;

public class SawDamage : MonoBehaviour
{
    [Tooltip("Testerenin saniyede vereceği hasar miktarı")]
    public float damagePerSecond = 20f;

    // Oyuncu testerenin içinde (trigger alanında) kaldığı sürece çalışır
    private void OnTriggerStay2D(Collider2D other)
    {
        // Temas eden objenin "Player" etiketi olup olmadığını kontrol et
        if (other.CompareTag("Player"))
        {
            // Player objesinin üzerindeki PlayerHealth bileşenini bul
            PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();

            // Eğer bileşen bulunduysa, zamanla orantılı (pürüzsüz) şekilde hasar ver
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }
}