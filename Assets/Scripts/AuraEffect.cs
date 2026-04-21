using UnityEngine;

public class AuraEffect : MonoBehaviour
{
    [Header("Dalgalanma Ayarları")]
    public float pulseHizi = 4f;       // Büyüme küçülme hızı
    public float pulseMiktari = 0.2f;  // Ne kadar büyüyecek
    
    [Header("Titreme (Jitter) Ayarları")]
    public float titremeSiddeti = 0.05f; // Puck üzerindeki kayma miktarı
    public float alfaFlickerHizi = 10f;  // Parlama/Sönme hızı

    private Vector3 orjinalScale;
    private SpriteRenderer spriteRenderer;
    private Color orjinalColor;

    void Start()
    {
        orjinalScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) orjinalColor = spriteRenderer.color;
    }

    void Update()
    {
        // 1. Nabız Etkisi (Büyüme - Küçülme)
        float scaleMod = Mathf.PingPong(Time.time * pulseHizi, pulseMiktari);
        transform.localScale = orjinalScale + new Vector3(scaleMod, scaleMod, 0);

        // 2. Titreme Etkisi (Rastgele Kayma)
        // Yerel pozisyonu sürekli çok küçük rastgele değerlerle sarsıyoruz
        transform.localPosition = (Vector3)Random.insideUnitCircle * titremeSiddeti;

        // 3. Parlama Efekti (Alpha Flicker)
        if (spriteRenderer != null)
        {
            float alfa = Mathf.PingPong(Time.time * alfaFlickerHizi, 0.5f) + 0.5f;
            Color yeniRenk = orjinalColor;
            yeniRenk.a = alfa;
            spriteRenderer.color = yeniRenk;
        }
    }
}