using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Transform currentTarget;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentTarget = pointB; 
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Y ekseninde (yukarı-aşağı) robotun kendi yüksekliğini koruyoruz ki havaya kalkmaya çalışmasın.
        // Sadece X ekseninde (sağ-sol) hedefin pozisyonunu alıyoruz.
        Vector2 targetPosition = new Vector2(currentTarget.position.x, transform.position.y);
        
        // Robotu yeni hedef pozisyona doğru hareket ettiriyoruz
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Mesafeyi SADECE yatayda (X ekseninde) ölçüyoruz. Mathf.Abs mutlak değer alır (eksi çıkmasını önler).
        float distanceX = Mathf.Abs(transform.position.x - currentTarget.position.x);

        // Yatayda hedefe çok yaklaştıysa yön değiştir
        if (distanceX < 0.5f)
        {
            if (currentTarget == pointA)
            {
                currentTarget = pointB;
                spriteRenderer.flipX = false; // Sağa dön
            }
            else 
            {
                currentTarget = pointA;
                spriteRenderer.flipX = true; // Sola dön
            }
        }
    }
}