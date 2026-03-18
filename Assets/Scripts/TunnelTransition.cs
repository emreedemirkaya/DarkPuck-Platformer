using UnityEngine;
using UnityEngine.Rendering.Universal; // 2D Işık sistemi için

public class TunnelTransition : MonoBehaviour
{
    [Header("Kapatılacak Eski Alan")]
    public GameObject oldArea;

    [Header("Işık Ayarları")]
    public Light2D globalLight;
    public float tunnelLightIntensity = 0.1f; // Tünel karanlık seviyesi

    [Header("Kamera Ayarları")]
    public Camera mainCamera; // Sahnedeki kamerayı buraya bağlayacağız

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kapıdan geçen obje Player ise
        if (collision.CompareTag("Player"))
        {
            // 1. Eski haritayı kapat (Hata aldığın kısım, bunu koruyoruz)
            if (oldArea != null)
            {
                oldArea.SetActive(false);
            }

            // 2. Global ışığı kıs
            if (globalLight != null)
            {
                globalLight.intensity = tunnelLightIntensity;
            }

            // --- YENİ EKLENEN KISIM: Arka Planı Siyah Yap ---
            if (mainCamera != null)
            {
                // Kameranın arkasındaki boşluk rengini siyah yap
                mainCamera.backgroundColor = Color.black; 
            }
        }
    }
}