using UnityEngine;

public class GlitchEngel : MonoBehaviour
{
    [Header("Hasar Ayarları")]
    public float hasarMiktari = 20f; // Oyuncudan gidecek can miktarı

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eğer çarpan şey Player veya CoreBattery ise
        if (other.CompareTag("Player") || other.name == "CoreBattery")
        {
            // 1. PİLİ DÜŞÜR
            AntiGravityZone odaKontrolcusu = Object.FindFirstObjectByType<AntiGravityZone>();
            if (odaKontrolcusu != null)
            {
                odaKontrolcusu.PiliDusur();
                Debug.Log("ENGELE ÇARPILDI! Pil düştü.");
            }

            // 2. CAN AZALT (Sadece çarpan şey oyuncuysa canını azalt)
            if (other.CompareTag("Player"))
            {
                PlayerHealth oyuncuCan = other.GetComponent<PlayerHealth>();
                if (oyuncuCan != null)
                {
                    oyuncuCan.TakeDamage(hasarMiktari);
                    Debug.Log("Oyuncu Hasar Aldı! Kalan canı azalıyor...");
                }
            }
        }
    }
}