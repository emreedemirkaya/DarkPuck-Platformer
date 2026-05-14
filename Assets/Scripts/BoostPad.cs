using UnityEngine;
using System.Collections;

public class BoostPad : MonoBehaviour
{
    [Header("Fırlatma Ayarları")]
    public float firlatmaGucu = 60f; 
    public float devreDisiKalmaSuresi = 0.5f;

    [Header("UI Referansları")]
    public GameObject pressShiftYazisi; // Müfettişten (Inspector) "Press Shift" objesini buraya sürükle

    [Header("Referanslar")]
    public MonoBehaviour oyuncuHareketScripti; 

    private bool oyuncuIceride = false;
    private Rigidbody2D playerRb;
    private bool firlatildiMi = false;

    void Start()
    {
        // Oyun başında yazının kapalı olduğundan emin olalım
        if (pressShiftYazisi != null) pressShiftYazisi.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            oyuncuIceride = true;
            playerRb = collision.GetComponent<Rigidbody2D>();
            
            // Oyuncu alana girdiğinde yazıyı göster
            if (pressShiftYazisi != null && !firlatildiMi) 
                pressShiftYazisi.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            oyuncuIceride = false;
            
            // Oyuncu alandan çıktığında yazıyı gizle
            if (pressShiftYazisi != null) 
                pressShiftYazisi.SetActive(false);
        }
    }

    void Update()
    {
        if (oyuncuIceride && !firlatildiMi)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(FirlatmaSureci());
            }
        }
    }

    IEnumerator FirlatmaSureci()
    {
        firlatildiMi = true;
        
        // Fırlatma başladığı an yazıyı gizle (Görüntü kirliliği olmasın)
        if (pressShiftYazisi != null) pressShiftYazisi.SetActive(false);

        if (playerRb != null)
        {
            EnergyOverload energy = playerRb.GetComponent<EnergyOverload>();
            if (energy != null) energy.EnerjiyiBaslat();

            if (oyuncuHareketScripti != null) oyuncuHareketScripti.enabled = false;

            playerRb.linearVelocity = Vector2.zero;
            playerRb.linearVelocity = new Vector2(firlatmaGucu, 0f);

            yield return new WaitForSeconds(devreDisiKalmaSuresi);

            if (oyuncuHareketScripti != null) oyuncuHareketScripti.enabled = true;
            
            yield return new WaitForSeconds(0.5f);
            firlatildiMi = false;
        }
    }
}