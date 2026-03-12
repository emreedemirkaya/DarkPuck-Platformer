using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Zemin Kontrolü")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb; // İŞTE BURADA: rb artık tüm script tarafından biliniyor
    private float moveInput;
    private bool isGrounded;
    private bool isKnockback = false; 

    void Start()
    {
        // rb'yi fizik bileşenine bağlıyoruz
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isKnockback) return; // Fırlatılma anında klavye girdisi alma

        moveInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

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
}