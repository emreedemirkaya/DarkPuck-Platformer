using UnityEngine;
using TMPro; // TextMeshPro kullanıyorsan bu gerekli

public class ScoreManager : MonoBehaviour
{
    // Diğer scriptlerden kolayca erişmek için Singleton yapısı
    public static ScoreManager instance; 

    public int score = 0;
    public TextMeshProUGUI scoreText; // Canvas'taki yazı objemiz

    private void Awake()
    {
        // Sahnede sadece tek bir ScoreManager olduğundan emin oluyoruz
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Yanlışlıkla birden fazla eklenirse fazladan olanı siler
        }
    }

    // Skoru artırmak için çağıracağımız fonksiyon
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    // Ekrandaki metni güncelleyen fonksiyon
    private void UpdateScoreUI()
    {
        if(scoreText != null)
        {
            scoreText.text = "SCORE:" + score;
        }
    }
}