using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menü Panelleri")]
    public GameObject rulesPanel; // Kurallar panelini buraya bağlayacağız

    public void PlayGame()
    {
        Time.timeScale = 1f; 
        if (PauseManager.isPaused) PauseManager.isPaused = false; 
        
        SceneManager.LoadScene("Level1"); 
    }

    public void QuitGame()
    {
        Debug.Log("Oyundan çıkıldı!");
        Application.Quit();
    }

    // --- YENİ EKLENEN KISIM ---
    
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