using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    [Header("Sağlık Ayarları")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthSlider;

    [Header("Görsel Efektler - Boss")]
    private SpriteRenderer bossSr;
    private Color bossOriginalColor;
    public float flashDuration = 0.1f;
    public int flashCount = 5;

    [Header("Geri Tepme Ayarları")]
    public float geriTepmeGucu = 40f;

    [Header("Ölüm ve Eşya Ayarları")]
    public GameObject anahtarPrefab; 
    public GameObject aktiflesecekKopru; // Müfettişten köprüyü buraya sürükle
    public float kopruGelisHizi = 1f; // Köprü kaç saniyede tamamen belirsin?

    void Start()
    {
        bossSr = GetComponent<SpriteRenderer>();
        if (bossSr != null) bossOriginalColor = bossSr.color;

        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }

        // Başlangıçta köprü collider'ını kapatalım ki görünmezken üzerinden geçilmesin
        if (aktiflesecekKopru != null)
        {
            Collider2D kopruCol = aktiflesecekKopru.GetComponent<Collider2D>();
            if (kopruCol != null) kopruCol.enabled = false;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (healthSlider != null) healthSlider.value = currentHealth;

        StopCoroutine(nameof(BossFlashEffect));
        StartCoroutine(nameof(BossFlashEffect));

        if (currentHealth <= 0) Die();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 contactNormal = collision.contacts[0].normal;
            if (contactNormal.y < -0.5f) return; 

            float impactForce = collision.relativeVelocity.magnitude;

            if (impactForce > 30f) 
            {
                // Can 20'den fazlaysa hem hasar ver hem oyuncuyu yanıp söndür
                if (currentHealth > 20f)
                {
                    TakeDamage(20f);
                    UygulaGeriTepme(collision.gameObject);

                    SpriteRenderer playerSr = collision.gameObject.GetComponent<SpriteRenderer>();
                    if (playerSr != null) StartCoroutine(PlayerFlashEffect(playerSr));
                }
                // Can 20 veya azsa (Son vuruş) EFEKT YAPMA
                else 
                {
                    TakeDamage(20f);
                    UygulaGeriTepme(collision.gameObject);
                }
            }
        }
    }

    void UygulaGeriTepme(GameObject playerObj)
    {
        Rigidbody2D playerRb = playerObj.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 bounceDirection = (playerObj.transform.position - transform.position).normalized;
            playerRb.linearVelocity = Vector2.zero;
            playerRb.AddForce(bounceDirection * geriTepmeGucu, ForceMode2D.Impulse);
        }
    }

    IEnumerator BossFlashEffect()
    {
        if (bossSr == null) yield break;
        for (int i = 0; i < flashCount; i++)
        {
            bossSr.color = Color.red;
            yield return new WaitForSeconds(flashDuration);
            bossSr.color = bossOriginalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    IEnumerator PlayerFlashEffect(SpriteRenderer pSr)
    {
        Color pOriginalColor = Color.white;
        for (int i = 0; i < flashCount; i++)
        {
            pSr.color = new Color(1f, 1f, 1f, 0.2f);
            yield return new WaitForSeconds(flashDuration);
            pSr.color = pOriginalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    void Die()
    {
        // 1. ANAHTAR DÜŞÜRME
        if (anahtarPrefab != null)
        {
            Instantiate(anahtarPrefab, transform.position, Quaternion.identity);
        }

        // 2. YAVAŞÇA GELEN KÖPRÜYÜ BAŞLAT
        if (aktiflesecekKopru != null)
        {
            // Köprüyü bağımsız bir scriptle veya Global bir sistemle açmalıyız 
            // Çünkü Boss objesi Destroy(gameObject) ile silindiğinde bu scriptteki Coroutine'ler durur.
            // Bu yüzden köprü açma komutunu köprünün kendi içindeki bir scripte veya basitçe Instantiate mantığına kurabiliriz.
            
            // En sağlam yol: Köprünün üzerindeki "Fade" scriptini tetiklemek.
            // Ama Boss silineceği için köprüyü sahneye "YENİDEN" spawn etmek en kolayıdır:
            aktiflesecekKopru.SetActive(true); 
            aktiflesecekKopru.SendMessage("KopruyuBelirt", SendMessageOptions.DontRequireReceiver);
        }

        Destroy(gameObject);
    }
}