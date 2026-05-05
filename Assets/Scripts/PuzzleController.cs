using UnityEngine;
using TMPro;

public class PuzzleController : MonoBehaviour
{
    [Header("Şifre Ayarları")]
    public int hedefSayi; 
    public PuzzleSwitch[] terminaller; 
    
    private int[] bitDegerleri = { 16, 8, 4, 2, 1 }; 

    [Header("Arayüz ve Köprü")]
    public TextMeshPro duvardakiKodYazisi; 
    public GameObject kopru;

    void Start()
    {
        if (kopru != null) kopru.SetActive(false); 

        hedefSayi = Random.Range(1, 32); 
        
        if (duvardakiKodYazisi != null)
        {
            duvardakiKodYazisi.text = "ERROR CODE: " + hedefSayi;
        }

        Debug.Log("Yeni Hedef Şifre: " + hedefSayi);
    }

    public void KontrolEt()
    {
        int mevcutToplam = 0;

        for (int i = 0; i < terminaller.Length; i++)
        {
            if (terminaller[i].mevcutDeger == 1)
            {
                mevcutToplam += bitDegerleri[i];
            }
        }

        Debug.Log("Şu anki toplam: " + mevcutToplam + " / Hedef: " + hedefSayi);

        // Şart kontrolü güncellendi:
        if (mevcutToplam == hedefSayi)
        {
            KopruyuAc();
        }
        else
        {
            // Eğer sayılar eşleşmiyorsa köprüyü hemen kapat!
            KopruyuKapat();
        }
    }

    void KopruyuAc()
    {
        if (kopru != null && !kopru.activeSelf)
        {
            kopru.SetActive(true);
            Debug.Log("ERİŞİM SAĞLANDI: Köprü Aktif!");
            
            if (duvardakiKodYazisi != null) duvardakiKodYazisi.text = "Successfull";
        }
    }

    // YENİ FONKSİYON: Şifre bozulduğunda çalışacak
    void KopruyuKapat()
    {
        if (kopru != null && kopru.activeSelf)
        {
            kopru.SetActive(false); // Köprüyü gizle
            Debug.Log("BAĞLANTI KOPTU: Köprü Kapandı!");
            
            // Duvardaki yazıyı "Successfull"dan tekrar hata koduna çevir
            if (duvardakiKodYazisi != null) duvardakiKodYazisi.text = "ERROR CODE:" +hedefSayi;
        }
    }
}