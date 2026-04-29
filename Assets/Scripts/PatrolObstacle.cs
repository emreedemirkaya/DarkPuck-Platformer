using UnityEngine;

public class PatrolObstacle : MonoBehaviour
{
    public Transform noktaA;
    public Transform noktaB;
    public float hiz = 2f;
    
    [Header("Hasar Ayarları")]
    public float hasarMiktari = 25f; // Dikenlerin hasarı

    private Vector3 hedef;

    void Start()
    {
        hedef = noktaB.position;
    }

    void Update()
    {
        // Platformu hedefe doğru hareket ettir
        transform.position = Vector3.MoveTowards(transform.position, hedef, hiz * Time.deltaTime);

        // Hedefe ulaştıysa yön değiştir
        if (Vector3.Distance(transform.position, hedef) < 0.1f)
        {
            hedef = hedef == noktaA.position ? noktaB.position : noktaA.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.name == "CoreBattery")
        {
            // 1. PİLİ DÜŞÜRME
            AntiGravityZone oda = Object.FindFirstObjectByType<AntiGravityZone>();
            if (oda != null)
            {
                oda.PiliDusur(); 
            }

            // 2. CAN AZALTMA (Sadece çarpan şey oyuncuysa canını azalt)
            if (other.CompareTag("Player"))
            {
                PlayerHealth oyuncuCan = other.GetComponent<PlayerHealth>();
                if (oyuncuCan != null)
                {
                    oyuncuCan.TakeDamage(hasarMiktari);
                    Debug.Log("Dikenlere Çarpıldı! Oyuncu Hasar Aldı.");
                }
            }
        }
    }
}