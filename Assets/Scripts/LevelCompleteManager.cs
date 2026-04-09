using UnityEngine;
using UnityEngine.SceneManagement; // Sahne geçişleri için bu satır şart!
using TMPro;

public class LevelCompleteManager : MonoBehaviour
{
    [Header("UI Elemanları")]
    public GameObject levelCompletePanel;
    public TextMeshProUGUI finalScoreText;

    public void ShowLevelComplete(int totalScore)
    {
        levelCompletePanel.SetActive(true); 
        finalScoreText.text = "Score: " + totalScore.ToString();
        Time.timeScale = 0f;
    }

    public void NextLevel()
    {
        // 1. Dondurulan zamanı tekrar normale döndür (Yoksa Level 2'de karakterin hareket edemez!)
        Time.timeScale = 1f; 
        
        // 2. Tırnak içindeki isim, Level 2 sahnenin adıyla BİREBİR aynı olmalı (Büyük/küçük harf duyarlıdır)
        SceneManager.LoadScene("Level2"); 
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}