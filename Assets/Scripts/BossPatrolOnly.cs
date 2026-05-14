using UnityEngine;

public class BossPatrolOnly : MonoBehaviour
{
    [Header("Sınır Noktaları")]
    public Transform noktaA; 
    public Transform noktaB; 

    [Header("Hareket Ayarları")]
    public float yurusHizi = 4f;

    private Rigidbody2D rb;
    private bool sagaGidiyor = true;
    private float solSinirX;
    private float sagSinirX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        rb.freezeRotation = true;
        rb.gravityScale = 5f;

        // Başlangıçta sınırları belirle
        if (noktaA != null && noktaB != null)
        {
            solSinirX = Mathf.Min(noktaA.position.x, noktaB.position.x);
            sagSinirX = Mathf.Max(noktaA.position.x, noktaB.position.x);
        }
    }

    void FixedUpdate()
    {
        if (noktaA == null || noktaB == null) return;

        // SADECE DEVRİYE GEZ
        if (sagaGidiyor)
        {
            rb.linearVelocity = new Vector2(yurusHizi, rb.linearVelocity.y);
            // Sağ sınıra yaklaştıysa geri dön
            if (transform.position.x >= sagSinirX - 0.1f) 
            {
                sagaGidiyor = false;
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(-yurusHizi, rb.linearVelocity.y);
            // Sol sınıra yaklaştıysa geri dön
            if (transform.position.x <= solSinirX + 0.1f) 
            {
                sagaGidiyor = true;
            }
        }

        // YÜZÜNÜ DÖNDÜR
        Flip();
    }

    void LateUpdate()
    {
        // KESİN SINIR KONTROLÜ (Dışarı taşmayı fiziksel olarak engeller)
        if (noktaA == null || noktaB == null) return;

        Vector3 pos = transform.position;
        if (pos.x < solSinirX) pos.x = solSinirX;
        if (pos.x > sagSinirX) pos.x = sagSinirX;
        transform.position = pos;
    }

    void Flip()
    {
        Vector3 newScale = transform.localScale;
        if (rb.linearVelocity.x > 0.1f)
            newScale.x = Mathf.Abs(newScale.x);
        else if (rb.linearVelocity.x < -0.1f)
            newScale.x = -Mathf.Abs(newScale.x);
        
        transform.localScale = newScale;
    }
}