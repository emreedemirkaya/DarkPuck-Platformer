using UnityEngine;

public class DroneAI : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f; // Kaç saniyede bir ateş edecek?
    public float bulletSpeed = 5f;

    private float nextFireTime;

    void Update()
    {
        if (player == null) return;

        // Drone her zaman oyuncuya doğru baksın (Yüzünü çevirme)
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

        // Ateş etme zamanı geldi mi?
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
{
    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

    // 1. Yönü hesapla (Oyuncu - Ateş Noktası)
    Vector2 direction = (player.position - firePoint.position).normalized;

    // 2. Merminin ucunu oyuncuya doğru döndür (Atan2 matematiği ile açı hesaplama)
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

    // 3. Mermiye hızı ver
    rb.linearVelocity = direction * bulletSpeed;

    Destroy(bullet, 3f);
}
}