using UnityEngine;
using TMPro; 
using UnityEngine.UI;

public class BlinkEffect : MonoBehaviour
{
    public float yanipSonmeHizi = 2f; // Ne kadar hızlı yanıp sönecek?
    public float minimumAlpha = 0.2f; // En sönük hali ne kadar şeffaf olsun?
    
    private TextMeshProUGUI tmpText;
    private Text standardText;

    void Start()
    {
        // Script hangisine takılıysa onu bulur
        tmpText = GetComponent<TextMeshProUGUI>();
        standardText = GetComponent<Text>();
    }

    void Update()
    {
        // Mathf.PingPong 0 ile 1 arasında sürekli gidip gelen bir değer üretir
        float alpha = Mathf.PingPong(Time.time * yanipSonmeHizi, 1f);
        
        // Alpha değerinin minimumAlpha'dan aşağı düşmemesini sağlayalım
        alpha = Mathf.Clamp(alpha, minimumAlpha, 1f);

        // Rengi güncelle
        if (tmpText != null)
        {
            Color c = tmpText.color;
            c.a = alpha;
            tmpText.color = c;
        }
        else if (standardText != null)
        {
            Color c = standardText.color;
            c.a = alpha;
            standardText.color = c;
        }
    }
}