using UnityEngine;

public class UnstablePlatform : MonoBehaviour
{
    [Header("Ayarlar")]
    public float kirilmaSuresi = 2f;
    public float titremeSiddeti = 0.05f;
    
    [Header("Algılama Alanı")]
    public Vector2 algilamaBoyutu = new Vector2(1.5f, 0.5f); // Platformun üstündeki alanın boyutu
    public Vector2 algilamaOfseti = new Vector2(0f, 0.5f);  // Platformun ne kadar üzerinde?
    public LayerMask oyuncuKatmani; 

    private float mevcutZaman = 0f;
    private bool dustuMu = false;
    private Vector3 orjinalPozisyon;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        orjinalPozisyon = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        if (dustuMu) return;

        // Platformun tam üzerindeki alanda bir Player var mı diye kontrol et
        Collider2D hit = Physics2D.OverlapBox((Vector2)transform.position + algilamaOfseti, algilamaBoyutu, 0, oyuncuKatmani);

        if (hit != null && hit.CompareTag("Player"))
        {
            // Enerjisi aktif mi?
            EnergyOverload energy = hit.GetComponent<EnergyOverload>();
            if (energy != null && energy.enerjiAktifMi)
            {
                mevcutZaman += Time.deltaTime;

                // Titreme
                transform.position = orjinalPozisyon + (Vector3)Random.insideUnitCircle * titremeSiddeti;
                
                // Renk değişimi
                spriteRenderer.color = Color.Lerp(Color.white, Color.red, mevcutZaman / kirilmaSuresi);

                if (mevcutZaman >= kirilmaSuresi)
                {
                    PlatformuDusur();
                }
                return; // Aşağıdaki 'Sıfırla' kısmına girmemesi için
            }
        }

        // Eğer buraya ulaştıysa oyuncu ya üstünde değildir ya da enerjisi yoktur
        if (mevcutZaman > 0) Sifirla();
    }

    void PlatformuDusur()
    {
        dustuMu = true;
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 4f;
            rb.linearVelocity = new Vector2(0, -5f);
        }
        GetComponent<Collider2D>().isTrigger = true;
    }

    void Sifirla()
    {
        mevcutZaman = 0f;
        transform.position = orjinalPozisyon;
        spriteRenderer.color = Color.white;
    }

    // Algılama alanını Unity içinde görmek için (Mavi kutu olarak görünür)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube((Vector2)transform.position + algilamaOfseti, algilamaBoyutu);
    }
   
   public void PlatformuSifirla()
{
    // 1. Durum değişkenlerini sıfırla
    dustuMu = false;
    mevcutZaman = 0f;
    // (oyuncuTemasEdiyor satırını buradan sildik)

    // 2. Fiziksel durumu düzelt
    if (rb != null)
    {
        rb.bodyType = RigidbodyType2D.Kinematic; // Tekrar havada asılı kalsın
        rb.linearVelocity = Vector2.zero;        // Hareketini durdur
        rb.angularVelocity = 0f;                 // Dönüşünü durdur
    }

    // 3. Pozisyon ve Görsellik
    transform.position = orjinalPozisyon;       // İlk yerine ışınla
    transform.rotation = Quaternion.identity;   // Yamuk düştüyse düzelt
    spriteRenderer.color = Color.white;          // Kırmızılığı sil
    
    // 4. Çarpışmayı tekrar aktif et
    GetComponent<Collider2D>().isTrigger = false;
    GetComponent<Collider2D>().enabled = true;
}
}