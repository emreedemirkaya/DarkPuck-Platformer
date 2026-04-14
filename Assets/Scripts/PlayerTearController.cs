using UnityEngine;

public class PlayerTearController : MonoBehaviour
{
    [Header("Mesafe Ayarları")]
    public Transform ekranTransform; // Pembe Pac-man ekranı
    public SpriteRenderer gozYasiSprite; // Gözyaşı objesinin SpriteRenderer'ı
    public float etkilesimMesafesi = 5f; // Ne kadar yakından ağlamaya başlasın?
    public float tamAglamaMesafesi = 1.5f; // Hangi mesafede tam görünür olsun?

    [Header("Akış Animasyonu")]
    public float akisHizi = 0.5f;        // Gözyaşının aşağı süzülme hızı
    public float akisMenzili = 0.3f;    // Damla ne kadar aşağı kayınca başa dönsün?
    
    private Vector3 baslangicLokalPos;
    private float mevcutInis = 0f;

    void Start()
    {
        // Gözyaşının (göz üzerindeki) orijinal yerini kaydet
        if (gozYasiSprite != null)
        {
            baslangicLokalPos = gozYasiSprite.transform.localPosition;
            
            // Başlangıçta şeffaf yap
            Color c = gozYasiSprite.color;
            c.a = 0f;
            gozYasiSprite.color = c;
        }
    }

    void Update()
    {
        if (ekranTransform == null || gozYasiSprite == null) return;

        float mesafe = Vector2.Distance(transform.position, ekranTransform.position);

        if (mesafe <= etkilesimMesafesi)
        {
            // 1. Şeffaflık Ayarı (Yaklaştıkça belirginleşir)
            float alpha = Mathf.InverseLerp(etkilesimMesafesi, tamAglamaMesafesi, mesafe);
            Color c = gozYasiSprite.color;
            c.a = alpha;
            gozYasiSprite.color = c;

            // 2. Akış Animasyonu
            // Sadece gözyaşı görünür olmaya başladığında (örn: %10 belirince) akmaya başlasın
            if (alpha > 0.1f)
            {
                mevcutInis += Time.deltaTime * akisHizi;

                // Eğer akış menzilini geçtiyse başa sar (Döngü)
                if (mevcutInis >= akisMenzili)
                {
                    mevcutInis = 0f;
                }

                // Gözyaşının pozisyonunu güncelle: 
                // Başlangıç noktasından 'mevcutInis' kadar aşağı (Y ekseninde eksi) kaydır
                gozYasiSprite.transform.localPosition = baslangicLokalPos + new Vector3(0, -mevcutInis, 0);
            }

            // 3. Titreme Efekti (Duygusal vurgu için çok yakındayken hıçkırık hissi)
            if (alpha > 0.9f)
            {
                gozYasiSprite.transform.localPosition += new Vector3(Random.Range(-0.01f, 0.01f), 0, 0);
            }
        }
        else
        {
            // Uzaktayken her şeyi sıfırla ve gizle
            Color c = gozYasiSprite.color;
            c.a = 0f;
            gozYasiSprite.color = c;
            
            mevcutInis = 0f;
            gozYasiSprite.transform.localPosition = baslangicLokalPos;
        }
    }
}