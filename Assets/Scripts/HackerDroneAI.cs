using UnityEngine;
using System.Collections;

public class HackerDroneAI : MonoBehaviour
{
    [Header("Devriye (Patrol) Ayarları")]
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2f;

    [Header("Zikzak (Dalgalanma) Ayarları")]
    public float zigzagSpeed = 1.5f;     
    public float zigzagHeight = 0.3f;  

    [Header("Hack, Bekleme ve HASAR Ayarları")]
    public float chaseSpeed = 3.5f;
    public float detectRange = 5f;
    public float hackDuration = 3f; 
    public float stunDuration = 10f; 
    
    // --- YENİ EKLENEN: Hasar Miktarı ---
    public float damageAmount = 15f; 

    private Transform currentTarget;
    private Transform player;
    private bool isChasing = false;
    
    // --- DURUMLAR ---
    private bool isReturning = false; 
    private bool isStunned = false;   
    
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentTarget = pointB;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        if (isStunned)
        {
            HoverInPlace();
            return;
        }

        if (isReturning)
        {
            ReturnToPointA();
            return; 
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectRange) isChasing = true;
        else isChasing = false;

        if (isChasing) ChasePlayer();
        else Patrol();
    }

    void Patrol()
    {
        float targetY = currentTarget.position.y + Mathf.Sin(Time.time * zigzagSpeed) * zigzagHeight;
        Vector2 targetPos = new Vector2(currentTarget.position.x, targetY);

        transform.position = Vector2.MoveTowards(transform.position, targetPos, patrolSpeed * Time.deltaTime);
        
        if (Mathf.Abs(transform.position.x - currentTarget.position.x) < 0.2f)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
            spriteRenderer.flipX = currentTarget == pointA;
        }
    }

    void ChasePlayer()
    {
        float targetY = player.position.y + Mathf.Sin(Time.time * zigzagSpeed) * zigzagHeight;
        Vector2 targetPos = new Vector2(player.position.x, targetY);

        transform.position = Vector2.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
        
        spriteRenderer.flipX = player.position.x < transform.position.x;
    }

    void ReturnToPointA()
    {
        float targetY = pointA.position.y + Mathf.Sin(Time.time * zigzagSpeed) * zigzagHeight;
        Vector2 targetPos = new Vector2(pointA.position.x, targetY);

        transform.position = Vector2.MoveTowards(transform.position, targetPos, patrolSpeed * Time.deltaTime);
        spriteRenderer.flipX = pointA.position.x < transform.position.x; 

        if (Mathf.Abs(transform.position.x - pointA.position.x) < 0.2f)
        {
            isReturning = false; 
            StartCoroutine(StunRoutine()); 
        }
    }

    void HoverInPlace()
    {
        float targetY = pointA.position.y + Mathf.Sin(Time.time * zigzagSpeed) * zigzagHeight;
        Vector2 targetPos = new Vector2(pointA.position.x, targetY);
        
        transform.position = Vector2.MoveTowards(transform.position, targetPos, patrolSpeed * Time.deltaTime);
    }

    // --- HASAR VE HACK KISMI (BURASI GÜNCELLENDİ) ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isReturning && !isStunned)
        {
            // Hem hareket scriptini (Hack için) hem de sağlık scriptini (Hasar için) buluyoruz
            PlayerController playerScript = collision.GetComponent<PlayerController>();
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>(); 
            
            // 1. Oyuncunun sistemlerine sız (Yönleri ters çevir)
            if (playerScript != null)
            {
                playerScript.ApplyHack(hackDuration); 
            }

            // 2. Oyuncuya fiziksel hasar ver (Canını azalt)
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }

            Debug.Log("Sisteme sızıldı ve can azaldı! Drone kapanıp A noktasına dönüyor...");
            
            // 3. Drone görevini yaptı, A noktasına dönüp şarja geçsin
            isReturning = true; 
            spriteRenderer.color = Color.red; 
        }
    }

    private IEnumerator StunRoutine()
    {
        isStunned = true; 
        Debug.Log("Drone A noktasına ulaştı, 10 saniye şarj oluyor...");

        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
        spriteRenderer.color = Color.white; 
        currentTarget = pointB; 
        Debug.Log("Drone şarj oldu ve yeniden aktif!");
    }
}