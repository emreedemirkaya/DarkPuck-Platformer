using UnityEngine;
using System.Collections;

public class LaserTracking : MonoBehaviour
{
    [Header("Ayarlar")]
    public float algilamaMenzili = 10f;
    public float isinTakipHizi = 8f;
    public float sarjSuresi = 1f; // Lazer hedefi tuttuktan kaç saniye sonra hasar vermeye başlasın?
    public float hasarMiktari = 15f; 
    public float hasarAraligi = 0.5f; 

    [Header("Sınır & Dönüş Ayarları")]
    public float xSinirDegeri = 10.8f; 
    public float maxDonusAcisi = 30f;  
    public float govdeDonusHizi = 5f;  
    public float gorselAciOfseti = 180f; 

    [Header("Görsel Genişlik")]
    public float sabitKalinlik = 0.05f; // Lazer başından sonuna kadar bu kalınlıkta kalacak

    [Header("Referanslar")]
    public Transform atisNoktasi; 
    public LineRenderer lineRenderer;
    public LayerMask lazerinCarpacagiKatmanlar;

    private Transform player;
    private float sarjTimer = 0f;
    private float hasarTimer = 0f; 
    private bool atesModu = false;
    private Vector2 guncelLazerYonu; 
    private bool oyuncuMenzilde = false;
    
    private float baslangicAcisiZ; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        baslangicAcisiZ = transform.eulerAngles.z;
    }

    void Update()
    {
        float mesafe = Vector2.Distance(transform.position, player.position);
        bool guvenliBolgede = player.position.x > xSinirDegeri;

        if (mesafe <= algilamaMenzili && !guvenliBolgede)
        {
            if (!oyuncuMenzilde)
            {
                guncelLazerYonu = (player.position - atisNoktasi.position).normalized;
                oyuncuMenzilde = true;
            }
            
            GovdeyiDondur();
            LazerMekanigi();
        }
        else
        {
            SistemiKapat();
        }
    }

    void GovdeyiDondur()
    {
        Vector3 yon = player.position - transform.position;
        float hedefAci = Mathf.Atan2(yon.y, yon.x) * Mathf.Rad2Deg;
        float duzeltilmisAci = hedefAci + gorselAciOfseti;

        float fark = Mathf.DeltaAngle(baslangicAcisiZ, duzeltilmisAci);
        float sinirlandirilmisFark = Mathf.Clamp(fark, -maxDonusAcisi, maxDonusAcisi);
        float sonAci = baslangicAcisiZ + sinirlandirilmisFark;

        Quaternion hedefRotasyon = Quaternion.Euler(0, 0, sonAci);
        transform.rotation = Quaternion.Slerp(transform.rotation, hedefRotasyon, govdeDonusHizi * Time.deltaTime);
    }

    void LazerMekanigi()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, atisNoktasi.position);

        // Lazerin kalınlığını sabit tutuyoruz
        lineRenderer.startWidth = sabitKalinlik;
        lineRenderer.endWidth = sabitKalinlik;

        Vector2 hedefYon = (player.position - atisNoktasi.position).normalized;
        guncelLazerYonu = Vector3.Slerp(guncelLazerYonu, hedefYon, isinTakipHizi * Time.deltaTime);

        RaycastHit2D hit = Physics2D.Raycast(atisNoktasi.position, guncelLazerYonu, algilamaMenzili, lazerinCarpacagiKatmanlar);

        if (hit.collider != null)
            lineRenderer.SetPosition(1, hit.point);
        else
            lineRenderer.SetPosition(1, (Vector2)atisNoktasi.position + (guncelLazerYonu * algilamaMenzili));

        sarjTimer += Time.deltaTime;

        // Şarj süresi dolduğunda
        if (sarjTimer >= sarjSuresi)
        {
            // Şarj dolduğu an ilk hasarı GECİKMESİZ vurması için timer'ı dolduruyoruz
            if (!atesModu)
            {
                atesModu = true;
                hasarTimer = hasarAraligi; 
            }

            hasarTimer += Time.deltaTime;

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                if (hasarTimer >= hasarAraligi)
                {
                    PlayerHealth health = hit.collider.GetComponent<PlayerHealth>();
                    if (health != null)
                    {
                        health.TakeDamage(hasarMiktari);
                        hasarTimer = 0; // Bir sonraki vuruş için süreyi sıfırla
                    }
                }
            }
        }
        else
        {
            atesModu = false;
            hasarTimer = 0;
        }
    }

    void SistemiKapat()
    {
        lineRenderer.enabled = false;
        sarjTimer = 0;
        hasarTimer = 0;
        atesModu = false;
        oyuncuMenzilde = false;
    }
}