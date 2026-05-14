using UnityEngine;

public class BossStabilMovement : MonoBehaviour
{
    [Header("Sınır Noktaları")]
    public Transform noktaA; 
    public Transform noktaB; 

    [Header("Zıplama Ayarları")]
    public float ziplamaGucu = 15f;
    public float ileriGuc = 7f;
    public float ziplamaAraligi = 2f; 
    public LayerMask zeminKatmani; 

    private Rigidbody2D rb;
    private Collider2D col;
    private Transform player;
    private float ziplamaZamanlayici;
    private bool yerdemi;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        ziplamaZamanlayici = ziplamaAraligi;
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (player == null || noktaA == null || noktaB == null) return;

        // 1. OTOMATİK ZEMİN KONTROLÜ (Mesafe ayarı gerektirmez)
        // Işını merkezden değil, direkt Boss'un ayak ucundan (Collider'ın en altından) gönderiyoruz
        Vector2 ayakHizasi = new Vector2(transform.position.x, transform.position.y - col.bounds.extents.y);
        yerdemi = Physics2D.Raycast(ayakHizasi, Vector2.down, 0.2f, zeminKatmani);
        
        Debug.DrawRay(ayakHizasi, Vector2.down * 0.2f, yerdemi ? Color.green : Color.red);

        // 2. YÜZÜNÜ DÖNME
        Vector3 newScale = transform.localScale;
        if (player.position.x > transform.position.x)
            newScale.x = Mathf.Abs(newScale.x);
        else
            newScale.x = -Mathf.Abs(newScale.x);
        transform.localScale = newScale;

        // 3. ZIPLAMA ZAMANLAYICI
        if (yerdemi)
        {
            ziplamaZamanlayici -= Time.deltaTime;
            if (ziplamaZamanlayici <= 0)
            {
                Zıpla();
                ziplamaZamanlayici = ziplamaAraligi;
            }
        }
    }

    // Fizik ve Sınır işlemleri burada olmalı
    void FixedUpdate() 
    {
        if (noktaA == null || noktaB == null) return;

        // 4. KESİN SINIR KONTROLÜ (FİZİK MOTORU ÜZERİNDEN)
        Vector2 pos = rb.position;
        pos.x = Mathf.Clamp(pos.x, noktaA.position.x, noktaB.position.x);
        rb.position = pos;

        // Yerdeyken kaymayı engelle (Sürtünme uygula)
        if (yerdemi && ziplamaZamanlayici > 0.2f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.8f, rb.linearVelocity.y);
        }
    }

    void Zıpla()
    {
        float yon = player.position.x > transform.position.x ? 1 : -1;

        // Zıplamadan önce sınırda mı kontrol et
        if (yon == -1 && rb.position.x <= noktaA.position.x + 0.5f) return;
        if (yon == 1 && rb.position.x >= noktaB.position.x - 0.5f) return;

        rb.linearVelocity = Vector2.zero; 
        rb.AddForce(new Vector2(yon * ileriGuc, ziplamaGucu), ForceMode2D.Impulse);
    }
}