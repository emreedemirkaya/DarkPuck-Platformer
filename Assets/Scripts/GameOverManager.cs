using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro yazılarını koddan değiştirmek için şart

public class GameOverManager : MonoBehaviour
{
    [Header("UI Elemanları")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    // Karakterin canı bittiğinde bu fonksiyonu çağıracağız
    public void ShowGameOver(int totalScore)
    {
        gameOverPanel.SetActive(true); // Paneli görünür yap
        finalScoreText.text = "TOTAL SKOR: " + totalScore.ToString(); // Skoru yazdır
        
        Time.timeScale = 0f; // Zamanı dondur (Pause menüsündeki gibi)
    }

    // YENİDEN BAŞLA butonuna basıldığında çalışacak
    public void RestartGame()
    {
        Time.timeScale = 1f; // Zamanı tekrar akıt
        // Şu anki bulunduğumuz sahneyi (Level1) baştan yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    // ANA MENÜ butonuna basıldığında çalışacak
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}