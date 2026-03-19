using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TwoWayPortal : MonoBehaviour
{
    [Header("Harita Alanları")]
    public GameObject cityArea;   // Şehir (Aydınlık) alanı
    public GameObject tunnelArea; // Tünel (Karanlık) alanı (varsa)

    [Header("Işık Ayarları")]
    public Light2D globalLight;
    public float cityLightIntensity = 1.0f;   // Şehrin ışık seviyesi (Aydınlık)
    public float tunnelLightIntensity = 0.1f; // Tünelin ışık seviyesi (Karanlık)

    // OnTriggerExit2D: Karakter kapının İÇİNDEN ÇIKTIĞINDA çalışır
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Eğer karakterin X pozisyonu, kapının X pozisyonundan BÜYÜKSE (Yani kapının SAĞINDAYSA)
            // Demek ki oyuncu Tünele girdi.
            if (collision.transform.position.x > transform.position.x)
            {
                if (cityArea != null) cityArea.SetActive(false);
                if (tunnelArea != null) tunnelArea.SetActive(true);
                
                if (globalLight != null) globalLight.intensity = tunnelLightIntensity;
                
                Debug.Log("Tünele girildi, şehir kapatıldı.");
            }
            // Eğer karakterin X pozisyonu, kapının X pozisyonundan KÜÇÜKSE (Yani kapının SOLUNDAYSA)
            // Demek ki oyuncu Şehre geri döndü.
            else
            {
                if (cityArea != null) cityArea.SetActive(true);
                if (tunnelArea != null) tunnelArea.SetActive(false); // Eğer tünele ait özel bir obje varsa kapatır
                
                if (globalLight != null) globalLight.intensity = cityLightIntensity;
                
                Debug.Log("Şehre dönüldü, şehir açıldı.");
            }
        }
    }
}