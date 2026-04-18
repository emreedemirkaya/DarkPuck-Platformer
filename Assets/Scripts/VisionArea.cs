using UnityEngine;

public class VisionArea : MonoBehaviour
{
    private SecurityCamera anaKamera;

    void Start()
    {
        anaKamera = GetComponentInParent<SecurityCamera>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // anaKamera silinmiş olabilir veya oyuncu yok edilmiş olabilir kontrolü
        if (anaKamera != null && collision != null && collision.CompareTag("Player"))
        {
            anaKamera.OyuncuTaramaAlaninda(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Hata buradaydı: Eğer collision veya anaKamera o an yok oluyorsa hata verir
        if (anaKamera != null && collision != null && collision.CompareTag("Player"))
        {
            anaKamera.OyuncuTaramadanCikti();
        }
    }

    // Eğer bu obje (ışık) bir şekilde deaktif olursa alarmı sustur
    private void OnDisable()
    {
        if (anaKamera != null)
        {
            anaKamera.OyuncuTaramadanCikti();
        }
    }
}