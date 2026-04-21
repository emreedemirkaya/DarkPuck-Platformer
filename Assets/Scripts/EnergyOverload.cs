using UnityEngine;

public class EnergyOverload : MonoBehaviour
{
    [Header("Enerji Ayarları")]
    public bool enerjiAktifMi = false; 
    public GameObject sariAuraGorseli;
    
    [Header("Bitiş Noktası")]
    public float bitisXKoordinati = 114f;

    void Start()
    {
        // Oyun başında enerjiyi ve görseli kapalı tut
        EnerjiyiKapat();
    }

    void Update()
    {
        // 114 X koordinatına ulaşınca enerjiyi otomatik kapat
        if (enerjiAktifMi && transform.position.x >= bitisXKoordinati)
        {
            EnerjiyiKapat();
            Debug.Log("Sektör geçildi: Enerji Stabilize.");
        }
    }

    public void EnerjiyiBaslat()
    {
        enerjiAktifMi = true;
        if (sariAuraGorseli != null) sariAuraGorseli.SetActive(true);
    }

    public void EnerjiyiKapat()
    {
        enerjiAktifMi = false;
        if (sariAuraGorseli != null) sariAuraGorseli.SetActive(false);
    }
}