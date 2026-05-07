using UnityEngine;

public class PlayerMagnetism : MonoBehaviour
{
    public enum Polarity { Positive, Negative, None }
    
    [Header("Durum")]
    public Polarity currentPolarity = Polarity.None;
    public bool inMagneticZone = false; 

    [Header("Görseller (Sprite Swap)")]
    public Sprite defaultSprite;   
    public Sprite positiveSprite;  
    public Sprite negativeSprite;  

    [Header("Yerçekimi Ayarları")]
    public float normalGravity = 4f;    
    public float blueGravity = 1f;      

    [Header("Manyetik Güç Ayarları")]
    public float magnetismStrength = 35f;      
    public float attractionStickiness = 15f;   

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator anim; // YENİ: Animasyon kontrolcüsü eklendi

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // YENİ: Animator'ı bul
    }

    void Start()
    {
        SetPolarity(Polarity.None);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && inMagneticZone)
        {
            if (currentPolarity == Polarity.Positive)
                SetPolarity(Polarity.Negative);
            else
                SetPolarity(Polarity.Positive);
        }
    }

    public void SetPolarity(Polarity newPolarity)
    {
        currentPolarity = newPolarity;
        spriteRenderer.color = Color.white; 

        if (currentPolarity == Polarity.Positive)
        {
            if(anim != null) anim.enabled = false; // YENİ: Kırmızı iken animasyonu durdur
            spriteRenderer.sprite = positiveSprite; 
            rb.gravityScale = normalGravity;
        }
        else if (currentPolarity == Polarity.Negative)
        {
            if(anim != null) anim.enabled = false; // YENİ: Mavi iken animasyonu durdur
            spriteRenderer.sprite = negativeSprite; 
            rb.gravityScale = blueGravity;   
        }
        else
        {
            if(anim != null) anim.enabled = true; // YENİ: Dışarı çıkınca/Sarı iken animasyonu tekrar başlat
            // Animator açılacağı için defaultSprite'ı otomatik kendi atayacaktır ama biz yine de yazalım:
            spriteRenderer.sprite = defaultSprite; 
            rb.gravityScale = normalGravity; 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MagneticZone"))
        {
            inMagneticZone = true;
            SetPolarity(Polarity.Negative); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MagneticZone"))
        {
            inMagneticZone = false;
            SetPolarity(Polarity.None); 
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!inMagneticZone) return;

        if ((collision.gameObject.CompareTag("Positive") && currentPolarity == Polarity.Positive) ||
            (collision.gameObject.CompareTag("Negative") && currentPolarity == Polarity.Negative))
        {
            Vector2 pushDir = (transform.position.y > collision.transform.position.y) ? Vector2.up : Vector2.down;
            rb.AddForce(pushDir * magnetismStrength, ForceMode2D.Force);
        }
        else if ((collision.gameObject.CompareTag("Positive") && currentPolarity == Polarity.Negative) ||
                 (collision.gameObject.CompareTag("Negative") && currentPolarity == Polarity.Positive))
        {
            Vector2 pullDir = (transform.position.y > collision.transform.position.y) ? Vector2.down : Vector2.up;
            rb.AddForce(pullDir * attractionStickiness, ForceMode2D.Force);
        }
    }
}