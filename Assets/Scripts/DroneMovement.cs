using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    [Header("Devriye Ayarları")]
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    [Header("Süzülme (Hover) Ayarları")]
    public float hoverAmplitude = 0.5f; // Yukarı-aşağı ne kadar gidecek?
    public float hoverFrequency = 2f;  // Ne kadar hızlı süzülecek?

    private Transform currentTarget;
    private float startY;

    void Start()
    {
        currentTarget = pointB;
        startY = transform.position.y;
    }

    void Update()
    {
        // 1. Sağa-Sola Hareket (X Ekseni)
        Vector2 targetPosition = new Vector2(currentTarget.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // 2. Havada Süzülme Efekti (Y Ekseni - Sinüs Dalgası)
        float newY = startY + Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // 3. Hedefe Vardı mı? (Sadece X'e bakıyoruz)
        if (Mathf.Abs(transform.position.x - currentTarget.position.x) < 0.1f)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
            
            // Drone'un yüzünü gittiği yöne çevir
            Flip();
        }
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}