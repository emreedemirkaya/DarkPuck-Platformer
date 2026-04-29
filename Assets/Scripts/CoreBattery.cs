using UnityEngine;

public class CoreBattery : MonoBehaviour
{
    [Header("Mekanik Ayarları")]
    public float asagiCekmeHizi = 3f; // Oyuncunun pili zorla aşağı çekme hızı
    public float yukariFirlamaHizi = 8f; // Tuş bırakılınca tavana geri fırlama hızı
    public Transform soketNoktasi;
    public GameObject cikisKapisi; // Sağdaki açılacak kapı

    private bool tutulduMu = false;
    private bool yerlestiMi = false;
    private float tavanYPos; // Pilin tavandaki orijinal konumu

    private Transform oyuncu;
    private Rigidbody2D oyuncuRb;
    private float normalYercekimi = 3f;

    void Start()
    {
        // Pilin oyuna başladığı tavan noktasını hafızaya al
        tavanYPos = transform.position.y;
    }

    void Update()
    {
        // Eğer pil tutuluyorsa ve henüz yuvaya oturmadıysa
        if (tutulduMu && !yerlestiMi)
        {
            // 1. Oyuncuyu pile kilitle (pilin hemen altında dursun)
            oyuncu.position = new Vector3(transform.position.x, transform.position.y - 0.6f, oyuncu.position.z);

            // 2. Oyuncu Aşağı tuşuna (veya S'ye) basarsa pili zorla aşağı çek
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.down * asagiCekmeHizi * Time.deltaTime);
            }
            // 3. Tuşu bırakırsa manyetik çekimle hızla tavana geri fırla
            else
            {
                transform.Translate(Vector3.up * yukariFirlamaHizi * Time.deltaTime);
            }

            // 4. Pilin tavandan daha yukarıya (çatıdan dışarı) çıkmasını engelle
            if (transform.position.y > tavanYPos)
            {
                transform.position = new Vector3(transform.position.x, tavanYPos, transform.position.z);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Görev çoktan bittiyse hiçbir şey yapma
        if (yerlestiMi) return;

        // Oyuncu pile dokunursa
        if (other.CompareTag("Player") && !tutulduMu)
        {
            tutulduMu = true;
            oyuncu = other.transform;
            oyuncuRb = other.GetComponent<Rigidbody2D>();

            // Oyuncunun kendi fiziğini iptal et, çünkü artık pilin kontrolüne girdi
            oyuncuRb.gravityScale = 0f; 
            oyuncuRb.linearVelocity = Vector2.zero;
        }

        // Eğer aşağı çekerken Pil, Soket'e dokunursa görevi bitir
        if (other.CompareTag("Socket") && tutulduMu)
        {
            GoreviTamamla();
        }
    }

    void GoreviTamamla()
    {
        yerlestiMi = true;
        tutulduMu = false;

        // Pili tam olarak prizin (soketin) merkezine oturt
        transform.position = soketNoktasi.position;

        // Oyuncuya yerçekimini ve hareket özgürlüğünü geri ver
        if (oyuncuRb != null)
        {
            oyuncuRb.gravityScale = normalYercekimi;
        }

        // Çıkış kapısını aktif hale getir
        if (cikisKapisi != null)
        {
            cikisKapisi.SetActive(true);
        }

        Debug.Log("GÜÇ SAĞLANDI! Yerçekimi Normale Döndü, Kapı Açıldı.");
    }
}