using UnityEngine;
using TMPro;

public class PuzzleSwitch : MonoBehaviour
{
    [Header("Terminal Ayarları")]
    public int mevcutDeger = 0;
    public TextMeshPro degeriGosterenYazi;
    public GameObject pressEYazisi;

    [Header("Ses Ayarları")]
    public AudioSource sesKaynagi; // Eklediğimiz hoparlör
    public AudioClip degisimSesi;  // Çalınacak olan ses dosyası

    private bool oyuncuIceride = false;
    private PuzzleController kontrolcu;

    void Start()
    {
        kontrolcu = Object.FindFirstObjectByType<PuzzleController>();
        
        if (degeriGosterenYazi != null) degeriGosterenYazi.text = mevcutDeger.ToString();
        if (pressEYazisi != null) pressEYazisi.SetActive(false);
    }

    void Update()
    {
        if (oyuncuIceride && Input.GetKeyDown(KeyCode.E))
        {
            // 1. Değeri Değiştir
            mevcutDeger = (mevcutDeger == 0) ? 1 : 0;
            if (degeriGosterenYazi != null) degeriGosterenYazi.text = mevcutDeger.ToString();
            
            // 2. Sesi Çal
            if (sesKaynagi != null && degisimSesi != null)
            {
                // PlayOneShot sesi kesmeden üst üste çalabilmeyi sağlar
                sesKaynagi.PlayOneShot(degisimSesi); 
            }

            // 3. Ana Kontrolcüye Haber Ver
            if (kontrolcu != null) kontrolcu.KontrolEt();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            oyuncuIceride = true;
            if (pressEYazisi != null) pressEYazisi.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            oyuncuIceride = false;
            if (pressEYazisi != null) pressEYazisi.SetActive(false);
        }
    }
}