using UnityEngine;

public class PlayerTearController : MonoBehaviour
{
    public Transform ekranTransform; // Pembe Pac-man ekranı
    public SpriteRenderer gozYasiSprite; // Gözyaşı objesinin SpriteRenderer'ı
    public float etkilesimMesafesi = 5f; // Ne kadar yakından ağlamaya başlasın?
    public float tamAglamaMesafesi = 1.5f; // Hangi mesafede gözyaşı tam görünür olsun?

    void Start()
    {
        // Başlangıçta gözyaşını şeffaf yap
        if (gozYasiSprite != null)
        {
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
            // Mesafeyi 0-1 arası bir şeffaflık değerine dönüştür
            // Yaklaştıkça (mesafe azaldıkça) şeffaflık (alpha) artar
            float alpha = Mathf.InverseLerp(etkilesimMesafesi, tamAglamaMesafesi, mesafe);
            
            Color c = gozYasiSprite.color;
            c.a = alpha;
            gozYasiSprite.color = c;

            // Eğer çok yakındaysa küçük bir titreme efekti
            if (alpha > 0.8f)
            {
                gozYasiSprite.transform.localPosition = new Vector3(
                    Random.Range(-0.02f, 0.02f), 
                    gozYasiSprite.transform.localPosition.y, 
                    0);
            }
        }
        else
        {
            // Uzaktaysa tamamen şeffaf kal
            Color c = gozYasiSprite.color;
            c.a = 0f;
            gozYasiSprite.color = c;
        }
    }
}