using UnityEngine;

public class BossWalkingMovement : MonoBehaviour
{
    [Header("Sınır Noktaları")]
    public Transform noktaA; 
    public Transform noktaB; 

    [Header("Hareket Ayarları")]
    public float yurusHizi = 4f;
    public float takipHizi = 6f;
    public float takipMenzili = 12f;

    private Rigidbody2D rb;
    private Transform player;
    private bool sagaGidiyor = true;
    private float solSinirX;
    private float sagSinirX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        rb.freezeRotation = true;
        rb.gravityScale = 5f;

        // Sınır değerlerini başlangıçta belirle (Hata payını azaltmak için)
        if (noktaA != null && noktaB != null)
        {
            solSinirX = Mathf.Min(noktaA.position.x, noktaB.position.x);
            sagSinirX = Mathf.Max(noktaA.position.x, noktaB.position.x);
        }
    }

    void FixedUpdate()
    {
        if (player == null || noktaA == null || noktaB == null) return;

        float mesafe = Vector2.Distance(transform.position, player.position);

        if (mesafe <= takipMenzili)
        {
            TakipEt();
        }
        else
        {
            DevriyeGez();
        }

        // YÖNÜ AYARLA
        Flip();
    }

    // KRİTİK NOKTA: LateUpdate, fizik ve hareket hesaplamalarından SONRA çalışır.
    // Boss nereye gitmeye çalışırsa çalışsın, burada onu sınırın içine hapsederiz.
    void LateUpdate()
    {
        if (noktaA == null || noktaB == null) return;

        Vector3 pos = transform.position;
        
        // Eğer Boss sınırların dışına çıktıysa, onu anında sınıra geri çek
        if (pos.x < solSinirX) pos.x = solSinirX;
        if (pos.x > sagSinirX) pos.x = sagSinirX;

        transform.position = pos;
    }

    void DevriyeGez()
    {
        if (sagaGidiyor)
        {
            rb.linearVelocity = new Vector2(yurusHizi, rb.linearVelocity.y);
            if (transform.position.x >= sagSinirX - 0.2f) sagaGidiyor = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(-yurusHizi, rb.linearVelocity.y);
            if (transform.position.x <= solSinirX + 0.2f) sagaGidiyor = true;
        }
    }

    void TakipEt()
    {
        float yon = player.position.x > transform.position.x ? 1 : -1;
        
        // Takip ederken bile sınır kontrolü yapalım ki hızıyla sınırın dışına taşmasın
        if (yon == 1 && transform.position.x >= sagSinirX)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }
        if (yon == -1 && transform.position.x <= solSinirX)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(yon * takipHizi, rb.linearVelocity.y);
    }

    void Flip()
    {
        Vector3 newScale = transform.localScale;
        // Oyuncu takip menzilindeyse her zaman oyuncuya baksın
        if (Vector2.Distance(transform.position, player.position) <= takipMenzili)
        {
            if (player.position.x > transform.position.x) newScale.x = Mathf.Abs(newScale.x);
            else newScale.x = -Mathf.Abs(newScale.x);
        }
        // Değilse gittiği yöne baksın
        else
        {
            if (rb.linearVelocity.x > 0.1f) newScale.x = Mathf.Abs(newScale.x);
            else if (rb.linearVelocity.x < -0.1f) newScale.x = -Mathf.Abs(newScale.x);
        }
        transform.localScale = newScale;
    }
}