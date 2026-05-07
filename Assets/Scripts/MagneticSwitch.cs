using UnityEngine;

public class MagneticSwitch : MonoBehaviour
{
    [Header("Şalter Ayarları")]
    public PlayerMagnetism.Polarity requiredPolarity; 
    public bool isActivated = false;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        // Başlangıçta %20 şeffaflık
        Color c = sr.color;
        c.a = 50f / 255f; 
        sr.color = c;
    }

    // Sadece İLK DEĞDİĞİ AN (Çarpma anı) kontrol edilir
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Eğer zaten aktifse veya Player değilse çık
        if (isActivated || !collision.CompareTag("Player")) return;

        PlayerMagnetism playerMag = collision.GetComponent<PlayerMagnetism>();

        if (playerMag != null)
        {
            // Çarpma anında rengi doğru mu?
            if (playerMag.currentPolarity == requiredPolarity)
            {
                ActivateSwitch();
            }
            else
            {
                // Yanlış renkte çarptıysa konsola yaz (Test için)
                Debug.Log("Yanlış renkle çarptın! Şalter aktif olmadı.");
            }
        }
    }

    void ActivateSwitch()
    {
        isActivated = true;
        
        // Şeffaflığı fulle
        Color c = sr.color;
        c.a = 1f; 
        sr.color = c;

        Debug.Log("<color=green>ŞALTER DARBE İLE AKTİF EDİLDİ!</color>");

        if (MagneticDoorManager.Instance != null)
        {
            MagneticDoorManager.Instance.CheckAllSwitches();
        }
    }
}