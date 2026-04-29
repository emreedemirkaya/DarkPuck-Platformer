using UnityEngine;
using TMPro;

public class PuzzleController : MonoBehaviour
{
    [Header("Şifre Ayarları")]
    public int hedefSayi; // Rastgele belirlenecek sayı (0-31)
    public PuzzleSwitch[] terminaller; // 5 adet terminal buraya gelecek
    
    // Her terminalin temsil ettiği değer: 16 - 8 - 4 - 2 - 1
    private int[] bitDegerleri = { 16, 8, 4, 2, 1 }; 

    [Header("Arayüz ve Köprü")]
    public TextMeshPro duvardakiKodYazisi; // "KOD: [Sayı]" yazan yer
    public GameObject kopru;

    void Start()
    {
        if (kopru != null) kopru.SetActive(false); 

        // 1. Oyuna her başlandığında 1 ile 31 arasında rastgele bir sayı belirle
        hedefSayi = Random.Range(1, 32); 
        
        // 2. Duvardaki yazıyı güncelle
        if (duvardakiKodYazisi != null)
        {
            duvardakiKodYazisi.text = "ERROR CODE: " + hedefSayi;
        }

        Debug.Log("Yeni Hedef Şifre: " + hedefSayi);
    }

    public void KontrolEt()
    {
        int mevcutToplam = 0;

        // 3. Her terminali kontrol et ve aktif olanların (1 olanların) değerini topla
        for (int i = 0; i < terminaller.Length; i++)
        {
            if (terminaller[i].mevcutDeger == 1)
            {
                mevcutToplam += bitDegerleri[i];
            }
        }

        Debug.Log("Şu anki toplam: " + mevcutToplam + " / Hedef: " + hedefSayi);

        // 4. Eğer toplam hedef sayıya eşitse köprüyü aç
        if (mevcutToplam == hedefSayi)
        {
            KopruyuAc();
        }
    }

    void KopruyuAc()
    {
        if (kopru != null && !kopru.activeSelf)
        {
            kopru.SetActive(true);
            Debug.Log("ERİŞİM SAĞLANDI: Köprü Aktif!");
            
            // İstersen duvardaki yazıyı "SİSTEM AÇILDI" olarak değiştirebilirsin
            if (duvardakiKodYazisi != null) duvardakiKodYazisi.text = "Successfull";
        }
    }
}