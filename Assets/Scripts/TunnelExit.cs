using UnityEngine;
using UnityEngine.Rendering.Universal; 

public class TunnelExit : MonoBehaviour
{
    [Header("Sahne Geçiş Referansları")]
    public GameObject cityArea;           
    // tunnelDarkness değişkenini tamamen sildik çünkü artık ona müdahale etmeyeceğiz!
    public Light2D globalLight;           

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null && player.hasKey)
            {
                Debug.Log("Kapı açıldı! Tünelden çıkılıyor, ışıklar açıldı.");
                
                // 1. Sadece şehri geri getiriyoruz. Tünelin siyah arka planına DOKUNMUYORUZ!
                if (cityArea != null) cityArea.SetActive(true);
                
                // 2. Işığı eski haline (1'e) getir
                if (globalLight != null) globalLight.intensity = 1f;

                // 3. Kapının kilidini aç (duvarı kaldır)
                GetComponent<Collider2D>().enabled = false; 
            }
            else
            {
                Debug.Log("Kapı Kilitli! Önce yukarıdaki anahtarı bulmalısın.");
            }
        }
    }
}