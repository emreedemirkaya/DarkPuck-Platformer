using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class HackTerminal : MonoBehaviour
{
    [Header("Sistem Ayarları")]
    public float hackSuresi = 3f;
    public float firlatmaGucu = 40f;

    [Header("UI Referansları")]
    public GameObject pressEYazisi;   
    public GameObject hackYazisi;     
    public Slider hackBar;           
    
    [Header("Referanslar")]
    public GameObject sagCizgi; 
    public MonoBehaviour oyuncuHareketScripti;

    private float mevcutSure = 0f;
    private bool oyuncuIceride = false;
    private Rigidbody2D playerRb;
    private bool hacklendiMi = false; // Sürecin tekrar etmemesi için

    void Start()
    {
        if (pressEYazisi != null) pressEYazisi.SetActive(false);
        if (hackYazisi != null) hackYazisi.SetActive(false);
        if (hackBar != null) hackBar.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            oyuncuIceride = true;
            playerRb = collision.GetComponent<Rigidbody2D>();
            
            if (pressEYazisi != null && !hacklendiMi) pressEYazisi.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            oyuncuIceride = false;
            mevcutSure = 0f;
            HepsiGizle();
        }
    }

    void Update()
    {
        if (oyuncuIceride && !hacklendiMi)
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (pressEYazisi != null) pressEYazisi.SetActive(false);
                if (hackYazisi != null) hackYazisi.SetActive(true);
                if (hackBar != null) hackBar.gameObject.SetActive(true);

                mevcutSure += Time.deltaTime;
                if (hackBar != null) hackBar.value = mevcutSure / hackSuresi;

                if (mevcutSure >= hackSuresi)
                {
                    hacklendiMi = true; // Tekrar hacklenmesini engelle
                    StartCoroutine(FirlatmaSureci());
                }
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                mevcutSure = 0f;
                if (hackBar != null) hackBar.value = 0f;
                if (pressEYazisi != null) pressEYazisi.SetActive(true);
                if (hackYazisi != null) hackYazisi.SetActive(false);
                if (hackBar != null) hackBar.gameObject.SetActive(false);
            }
        }
    }

    void HepsiGizle()
    {
        if (pressEYazisi != null) pressEYazisi.SetActive(false);
        if (hackYazisi != null) hackYazisi.SetActive(false);
        if (hackBar != null) hackBar.gameObject.SetActive(false);
    }

    IEnumerator FirlatmaSureci()
    {
        HepsiGizle();
        if (sagCizgi != null) sagCizgi.SetActive(false);
        
        if (playerRb != null)
        {
            // 1. ADIM: Oyuncunun üzerindeki Enerji Overload sistemini bul ve başlat
            EnergyOverload energy = playerRb.GetComponent<EnergyOverload>();
            if (energy != null)
            {
                energy.EnerjiyiBaslat();
            }

            // 2. ADIM: Fırlatma mekaniği
            if (oyuncuHareketScripti != null) oyuncuHareketScripti.enabled = false;
            
            playerRb.linearVelocity = Vector2.zero;
            playerRb.linearVelocity = new Vector2(firlatmaGucu, 0f);

            yield return new WaitForSeconds(0.5f);
            
            if (oyuncuHareketScripti != null) oyuncuHareketScripti.enabled = true;
            hacklendiMi = false; // Respawn sonrası tekrar kullanılabilmesi için
        }
    }
}