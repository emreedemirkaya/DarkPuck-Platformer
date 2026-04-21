using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    [Header("Işınlanma Ayarları")]
    public Vector2 respawnPos = new Vector2(20f, -5f);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. CAN SİSTEMİYLE BAĞLANTI VE %50 HESABI
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                // Sabit 50 değil, MAKSİMUM CANIN yarısını hesaplıyoruz
                float hasarMiktari = health.currentHealth * 0.5f; 
                
                // Hesaplanan hasarı sisteme gönderiyoruz
                health.TakeDamage(hasarMiktari);
                
                Debug.Log($"Puck düştü. Current Can: {health.currentHealth}, Alınan Hasar: {hasarMiktari}");
            }

            // 2. IŞINLAMA VE FİZİK SIFIRLAMA
            other.transform.position = new Vector3(respawnPos.x, respawnPos.y, other.transform.position.z);
            
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null) 
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            // 3. DİĞER SİSTEMLERİ SIFIRLA
            // Enerji (Aura) kapatma
            EnergyOverload energy = other.GetComponent<EnergyOverload>();
            if (energy != null) energy.EnerjiyiKapat();

            // Sahnede 'UnstablePlatform' scriptine sahip olan her şeyi bul ve resetle
            UnstablePlatform[] tumPlatformlar = FindObjectsByType<UnstablePlatform>(FindObjectsSortMode.None);
            foreach (UnstablePlatform platform in tumPlatformlar)
            {
                platform.PlatformuSifirla();
            }
        }
    }
}