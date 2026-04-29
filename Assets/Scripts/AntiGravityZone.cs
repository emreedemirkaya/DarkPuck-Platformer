using UnityEngine;

public class AntiGravityZone : MonoBehaviour
{
    [Header("Oda Objeleri")]
    public Transform batarya;
    public Transform soket;
    public GameObject engelleyenDuvar;
    public GameObject cikisKapisi;

    [Header("Fizik Ayarları")]
    public float tersYercekimiGucu = -3f;
    public float asagiYuzmeHizi = 6f;     
    public float asagiCekmeHizi = 4f;     
    public float yukariFirlamaHizi = 12f; 
    public float yatayHareketHizi = 5f; 

    [Header("Sınır Ayarları")]
    public float zeminYLimit = -6f; //Inspector'dan ayarlayacağımız zemin seviyesi

    private Transform iceridekiOyuncu;
    private Rigidbody2D oyuncuRb;
    private float normalYercekimi;
    private float tavanYPos;

    private bool pilTutulduMu = false;
    private bool gorevTamamlandi = false;
    private float pilTutmaBeklemeSuresi = 0f; 

    private SpriteRenderer bataryaSprite;

    void Start()
    {
        if (batarya != null) 
        {
            tavanYPos = batarya.position.y;
            bataryaSprite = batarya.GetComponent<SpriteRenderer>(); 
        }
        
        if (cikisKapisi != null) cikisKapisi.SetActive(false);
        if (engelleyenDuvar != null) engelleyenDuvar.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            iceridekiOyuncu = other.transform;
            oyuncuRb = other.GetComponent<Rigidbody2D>();
            normalYercekimi = oyuncuRb.gravityScale;
            
            if (!gorevTamamlandi)
            {
                oyuncuRb.gravityScale = tersYercekimiGucu;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && iceridekiOyuncu != null)
        {
            oyuncuRb.gravityScale = normalYercekimi;
            iceridekiOyuncu = null;
            oyuncuRb = null;
        }
    }

    void Update()
    {
        if (gorevTamamlandi || iceridekiOyuncu == null || batarya == null || soket == null) return;

        // BEKLEME SÜRESİ VE YANIP SÖNME EFEKTİ
        if (pilTutmaBeklemeSuresi > 0)
        {
            pilTutmaBeklemeSuresi -= Time.deltaTime;

            if (bataryaSprite != null)
            {
                bataryaSprite.enabled = Mathf.PingPong(Time.time * 15f, 1f) > 0.5f;
            }

            if (pilTutmaBeklemeSuresi <= 0 && bataryaSprite != null)
            {
                bataryaSprite.enabled = true;
            }
        }

        // 1. PİL TUTMA KONTROLÜ
        if (!pilTutulduMu && pilTutmaBeklemeSuresi <= 0 && Vector2.Distance(iceridekiOyuncu.position, batarya.position) < 1.5f)
        {
            pilTutulduMu = true;
            oyuncuRb.gravityScale = 0f;
            oyuncuRb.linearVelocity = Vector2.zero; 
        }

        // 2. PİL TUTULDUYSA
        if (pilTutulduMu)
        {
            float yatayGirdi = Input.GetAxis("Horizontal");
            batarya.Translate(Vector3.right * yatayGirdi * yatayHareketHizi * Time.deltaTime);

            iceridekiOyuncu.position = new Vector3(batarya.position.x, batarya.position.y - 0.8f, iceridekiOyuncu.position.z);

            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                batarya.Translate(Vector3.down * asagiCekmeHizi * Time.deltaTime);
            }
            else
            {
                batarya.Translate(Vector3.up * yukariFirlamaHizi * Time.deltaTime);
            }

            //SINIR KONTROLLERİ
            // Tavan Sınırı
            if (batarya.position.y > tavanYPos)
            {
                batarya.position = new Vector3(batarya.position.x, tavanYPos, batarya.position.z);
            }

            // YENİ: Milimetrik Zemin Sınırı
            if (batarya.position.y < zeminYLimit)
            {
                batarya.position = new Vector3(batarya.position.x, zeminYLimit, batarya.position.z);
            }

            // GÖREV BİTİŞ KONTROLÜ
            if (Vector2.Distance(batarya.position, soket.position) < 1.5f)
            {
                GoreviTamamla();
            }
        }
        // 3. PİL TUTULMUYORSA
        else
        {
            if (batarya.position.y < tavanYPos)
            {
                batarya.Translate(Vector3.up * yukariFirlamaHizi * Time.deltaTime);
            }

            if (oyuncuRb.gravityScale < 0)
            {
                if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                {
                    oyuncuRb.linearVelocity = new Vector2(oyuncuRb.linearVelocity.x, -asagiYuzmeHizi);
                }
            }
        }
    }

    public void PiliDusur()
    {
        if (pilTutulduMu)
        {
            pilTutulduMu = false;
            pilTutmaBeklemeSuresi = 1.0f; 

            if (oyuncuRb != null)
            {
                oyuncuRb.gravityScale = tersYercekimiGucu;
                oyuncuRb.linearVelocity = Vector2.zero; 
            }
        }
    }

    void GoreviTamamla()
    {
        gorevTamamlandi = true;
        pilTutulduMu = false;
        batarya.position = soket.position; 
        oyuncuRb.gravityScale = normalYercekimi; 
        
        if (bataryaSprite != null) bataryaSprite.enabled = true; 

        if (engelleyenDuvar != null) engelleyenDuvar.SetActive(false);
        if (cikisKapisi != null) cikisKapisi.SetActive(true);
    }
}