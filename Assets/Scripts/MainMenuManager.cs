using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menü Panelleri")]
    public GameObject rulesPanel; // Kurallar panelini buraya bağlayacağız

   void Start()
    {
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.score = 0;
            ScoreManager.instance.mevcutCan = 100f; // CAN BURADA FULKLENDİ
            Debug.Log("Ana Menü Yüklendi: Skor ve Can sıfırlandı!");
        }
    }

    public void PlayGame()
    {
        Time.timeScale = 1f; 
        if (PauseManager.isPaused) PauseManager.isPaused = false; 
        
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.score = 0;
            ScoreManager.instance.mevcutCan = 100f; // CAN BURADA FULLENDİ
        }

        SceneManager.LoadScene("Level1"); 
    }

    public void QuitGame()
    {
        Debug.Log("Oyundan çıkıldı!");
        Application.Quit();
    }
    
    // NASIL OYNANIR butonuna basılınca paneli aç
    public void OpenRules()
    {
        if (rulesPanel != null)
        {
            rulesPanel.SetActive(true);
        }
    }

    // GERİ butonuna basılınca paneli kapat
    public void CloseRules()
    {
        if (rulesPanel != null)
        {
            rulesPanel.SetActive(false);
        }
    }
}