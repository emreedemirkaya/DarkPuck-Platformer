using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Envanter")]
    public bool hasKey = false; // Karakterin anahtarı var mı?

    [Header("Zemin Kontrolü")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb; 
    private float moveInput;
    private bool isGrounded;
    private bool isKnockback = false; 
    
    // --- YENİ EKLENEN: Hack Durumu ---
    public bool isHacked = false;
    private SpriteRenderer spriteRenderer; // Görsel efekt (yeşil renk) için

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer'ı koda bağlıyoruz
    }

    void Update()
    {
        if (isKnockback) return; // Fırlatılma anında klavye girdisi alma

        moveInput = Input.GetAxisRaw("Horizontal");

        //  Virüs Bulaşınca Yönü Tersine Çevir 
        if (isHacked)
        {
            moveInput = -moveInput; // Sağ tuş sol, sol tuş sağ olur!
        }
        // -------------------------------------------------------------

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Hackliyken Zıplamayı İptal Et
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Karakterin yönünü çevirme (Sağ-Sol)
        if (moveInput > 0) transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (moveInput < 0) transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void FixedUpdate()
    {
        if (!isKnockback) 
        {
            Move();
        }
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    // Robotun çarptığında çağıracağı fonksiyon
    public void ApplyKnockback(Vector2 force, float duration)
    {
        if (!isKnockback)
        {
            StartCoroutine(KnockbackRoutine(force, duration));
        }
    }

    private IEnumerator KnockbackRoutine(Vector2 force, float duration)
    {
        isKnockback = true;
        rb.linearVelocity = Vector2.zero; // Hızı sıfırla ki fırlatma etkili olsun
        rb.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);

        isKnockback = false;
    }

    //Hacker Drone Sistemi
    public void ApplyHack(float duration)
    {
        if (!isHacked) 
        {
            StartCoroutine(HackRoutine(duration));
        }
    }

    private IEnumerator HackRoutine(float duration)
    {
        isHacked = true;
        Debug.Log("SİSTEME SIZILDI! Kontroller tersine döndü ve zıplama kilitlendi!");
        
        // Karakteri hacklendiğini belli etmek için "Matrix Yeşili" rengi olur
        Color originalColor = Color.white; 
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.green;
        }

        // Drone'un belirlediği süre kadar bu halde bekle
        yield return new WaitForSeconds(duration);

        // Süre bitince sistemi normale döndür
        isHacked = false;
        
        // Rengi eski haline döndür
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        
        Debug.Log("Sistem kurtarıldı. Kontroller normale döndü.");
    }
}