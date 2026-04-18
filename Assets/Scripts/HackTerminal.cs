using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; // TextMeshPro kullanıyorsanız mutlaka kalsın

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

    void Start()
    {
        // Başlangıçta her şeyi kapatalım
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
            
            // Sadece "Press E" yazısını göster
            if (pressEYazisi != null) pressEYazisi.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            oyuncuIceride = false;
            mevcutSure = 0f;
            
            // Alanı terk edince her şeyi gizle
            HepsiGizle();
        }
    }

    void Update()
    {
        if (oyuncuIceride)
        {
            // E tuşuna BASILI TUTULDUĞUNDA
            if (Input.GetKey(KeyCode.E))
            {
                // "Press E" gitsin, diğerleri gelsin
                if (pressEYazisi != null) pressEYazisi.SetActive(false);
                if (hackYazisi != null) hackYazisi.SetActive(true);
                if (hackBar != null) hackBar.gameObject.SetActive(true);

                mevcutSure += Time.deltaTime;
                if (hackBar != null) hackBar.value = mevcutSure / hackSuresi;

                if (mevcutSure >= hackSuresi)
                {
                    StartCoroutine(FirlatmaSureci());
                }
            }
            // E tuşu BIRAKILDIĞINDA
            else if (Input.GetKeyUp(KeyCode.E))
            {
                mevcutSure = 0f;
                if (hackBar != null) hackBar.value = 0f;
                
                // "Press E" geri gelsin, diğerleri gitsin
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