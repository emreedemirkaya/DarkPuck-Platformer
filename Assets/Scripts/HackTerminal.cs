using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; // Eğer TextMeshPro kullanıyorsan kalsın

public class HackTerminal : MonoBehaviour
{
    [Header("Sistem Ayarları")]
    public float hackSuresi = 3f;
    public float firlatmaGucu = 35f;

    [Header("Referanslar")]
    public GameObject sagCizgi; 
    public Slider hackBar;      
    public GameObject hackYazisi; 
    public MonoBehaviour oyuncuHareketScripti; // Karakterindeki hareket kodu (Örn: PlayerController)

    private float mevcutSure = 0f;
    private bool oyuncuIceride = false;
    private Rigidbody2D playerRb;

    private void Start()
    {
        // Başlangıçta her şeyi gizle
        if (hackBar != null) hackBar.gameObject.SetActive(false);
        if (hackYazisi != null) hackYazisi.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            oyuncuIceride = true;
            playerRb = collision.GetComponent<Rigidbody2D>();
            
            // DİKKAT: Burada artık SetActive(true) demiyoruz! 
            // Sadece oyuncunun içeride olduğunu kaydediyoruz.
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            oyuncuIceride = false;
            mevcutSure = 0f;
            // Alandan çıkınca her ihtimale karşı gizle
            if (hackBar != null) hackBar.gameObject.SetActive(false);
            if (hackYazisi != null) hackYazisi.SetActive(false);
        }
    }

    void Update()
    {
        // OYUNCU İÇERİDEYSE VE E TUŞUNA BASILI TUTUYORSA
        if (oyuncuIceride && Input.GetKey(KeyCode.E))
        {
            // İlk kez basıldığında barı ve yazıyı göster
            if (hackBar != null && !hackBar.gameObject.activeSelf) hackBar.gameObject.SetActive(true);
            if (hackYazisi != null && !hackYazisi.activeSelf) hackYazisi.SetActive(true);

            mevcutSure += Time.deltaTime;
            if (hackBar != null) hackBar.value = mevcutSure / hackSuresi;

            if (mevcutSure >= hackSuresi)
            {
                StartCoroutine(FirlatmaSureci());
            }
        }
        // TUŞU BIRAKIRSA VEYA ALANDAN ÇIKARSA
        else if (Input.GetKeyUp(KeyCode.E) || !oyuncuIceride)
        {
            mevcutSure = 0f;
            if (hackBar != null) 
            {
                hackBar.value = 0f;
                hackBar.gameObject.SetActive(false); // Tuşu bırakınca barı gizle
            }
            if (hackYazisi != null) hackYazisi.SetActive(false); // Tuşu bırakınca yazıyı gizle
        }
    }

    IEnumerator FirlatmaSureci()
    {
        // Fırlatma anında arayüzü kapat
        if (hackBar != null) hackBar.gameObject.SetActive(false);
        if (hackYazisi != null) hackYazisi.SetActive(false);
        
        if (sagCizgi != null) sagCizgi.SetActive(false);
        oyuncuIceride = false;

        if (playerRb != null)
        {
            if (oyuncuHareketScripti != null) oyuncuHareketScripti.enabled = false;

            playerRb.linearVelocity = Vector2.zero;
            playerRb.linearVelocity = new Vector2(firlatmaGucu, 0f);

            yield return new WaitForSeconds(0.5f);

            if (oyuncuHareketScripti != null) oyuncuHareketScripti.enabled = true;
        }
    }
}