using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SecurityCamera : MonoBehaviour
{
    // ... Mevcut tarama ve ışık değişkenlerin aynı kalsın ...
    [Header("Tarama Ayarları")]
    public float donusAcisi = 30f; 
    public float donusHizi = 1.5f; 
    
    [Header("Algılama ve Görsel")]
    public Light2D visionLight; 
    public float dashHiziEsigi = 15f;
    
    [Header("Işık Ayarları")]
    [Range(0, 10)] public float normalYogunluk = 2f;
    [Range(0, 10)] public float alarmYogunluk = 5f;

    [Header("Renkler")]
    public Color guvenliRenk = new Color(0f, 1f, 0.5f, 1f); 
    public Color alarmRengi = new Color(1f, 0.9f, 0f, 1f); 

    [Header("Tetiklenecek Nesneler")]
    public GameObject[] gizliLazerler;

    // YENİ EKLENEN SES DEĞİŞKENLERİ
    [Header("Ses Ayarları")]
    public AudioSource arkaPlanMuzigi; // Music Manager objesini buraya sürükle
    public AudioSource alarmSource;     // Kamera üzerindeki AudioSource
    public AudioClip alarmSesi;         // Oluşturduğumuz alarm .mp3 dosyasını buraya sürükle

    private bool oyuncuGoruldu = false;
    private float baslangicAcisiZ;

    void Start()
    {
        if (visionLight != null)
        {
            baslangicAcisiZ = visionLight.transform.localEulerAngles.z;
            visionLight.color = guvenliRenk;
            visionLight.intensity = normalYogunluk;
        }

        foreach(var lazer in gizliLazerler) if(lazer != null) lazer.SetActive(false);
    }

    
    void Update()
    {
        if (!oyuncuGoruldu && visionLight != null)
        {
            float aci = Mathf.PingPong(Time.time * donusHizi, donusAcisi * 2) - donusAcisi;
            visionLight.transform.localRotation = Quaternion.Euler(0, 0, baslangicAcisiZ + aci);
        }
    }

    public void OyuncuTaramaAlaninda(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !oyuncuGoruldu)
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null && rb.linearVelocity.magnitude < dashHiziEsigi)
            {
                AlarmCal();
            }
        }
    }

    // YENİ: Işıktan çıkınca çağrılacak fonksiyon
    public void OyuncuTaramadanCikti()
    {
        if (oyuncuGoruldu)
        {
            AlarmDurdur();
        }
    }

    void AlarmCal()
    {
        oyuncuGoruldu = true;
        if(visionLight != null) { visionLight.color = alarmRengi; visionLight.intensity = alarmYogunluk; }
        foreach(var lazer in gizliLazerler) lazer.SetActive(true);

        // SES İŞLEMLERİ
        if (arkaPlanMuzigi != null) arkaPlanMuzigi.Pause(); // Arka planı dondur
        if (alarmSource != null && alarmSesi != null)
        {
            alarmSource.clip = alarmSesi;
            alarmSource.Play();
        }
    }

    // Mevcut SecurityCamera kodunun içine şu fonksiyonu ekleyin veya güncelleyin:

private void OnDisable()
{
    // Eğer kamera bir şekilde kapanırsa veya sahne biterse alarmı zorla sustur
    if (oyuncuGoruldu)
    {
        AlarmDurdur();
    }
}

// AlarmDurdur fonksiyonunun içine de bir güvenlik ekleyelim
void AlarmDurdur()
{
    oyuncuGoruldu = false;
    if(visionLight != null) { visionLight.color = guvenliRenk; visionLight.intensity = normalYogunluk; }
    
    // Lazerleri kapatırken null kontrolü
    foreach(var lazer in gizliLazerler) 
    {
        if(lazer != null) lazer.SetActive(false);
    }

    // SES İŞLEMLERİ
    if (alarmSource != null && alarmSource.isPlaying) alarmSource.Stop(); 
    
    // Arka plan müziğini sadece durdurulmuşsa geri başlat
    if (arkaPlanMuzigi != null) arkaPlanMuzigi.UnPause(); 
}
}