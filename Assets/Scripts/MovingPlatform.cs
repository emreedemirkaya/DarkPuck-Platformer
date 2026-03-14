using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float speed = 2f;      // Platformun hızı
    public float distance = 2f;   // Ne kadar yukarı/aşağı gideceği

    private Vector3 startPosition;

    void Start()
    {
        // Platformun oyuna başladığı ilk merkezi konumu kaydediyoruz
        startPosition = transform.position;
    }

    void Update()
    {
        // Sinüs dalgası (Mathf.Sin) kullanarak yumuşak bir asansör hareketi oluşturuyoruz
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * distance;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    // Karakter platformun üstüne atladığında:
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Eğer çarpan şeyin etiketi "Player" ise, onu platformun alt objesi (çocuğu) yap
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    // Karakter platformdan zıplayıp ayrıldığında:
    private void OnCollisionExit2D(Collision2D collision)
{
    // Çarpışan obje Player ise
    if (collision.gameObject.CompareTag("Player"))
    {
        // Platform hala aktifse ve sahnede çalışıyorsa bağları kopar
        if (this.gameObject.activeInHierarchy)
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
}