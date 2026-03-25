using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelCompleteManager : MonoBehaviour
{
    [Header("UI Elemanları")]
    public GameObject levelCompletePanel;
    public TextMeshProUGUI finalScoreText;

    public void ShowLevelComplete(int totalScore)
    {
        levelCompletePanel.SetActive(true); // Paneli aç
        finalScoreText.text = "Total Score: " + totalScore.ToString();
        
        Time.timeScale = 0f; // Oyunu (zamanı) dondur
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        // Şimdilik 2. level olmadığı için Ana Menüye veya aynı levele döndürebilirsin
        SceneManager.LoadScene("MainMenu"); 
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}