using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    // Diğer scriptlerin oyunun durup durmadığını bilmesi için (static)
    public static bool isPaused = false; 
    
    [Header("Menü Arayüzü")]
    public GameObject pauseMenuUI;

    void Update()
    {
        // Klavyeden ESC tuşuna basıldığında (İstersen KeyCode.P de yapabilirsin)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume(); // Açıksa kapat
            }
            else
            {
                Pause(); // Kapalıysa aç
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Menüyü gizle
        Time.timeScale = 1f; // Zamanı normal hızına (1) döndür
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true); // Menüyü göster
        Time.timeScale = 0f; // Zamanı DURDUR (0) - Her şey donar!
        isPaused = true;
    }

    // "Ana Menü" butonuna basılınca çalışacak fonksiyon
    public void LoadMainMenu()
    {
        // DİKKAT: Ana menüye dönerken zamanı tekrar 1 yapmalıyız, yoksa ana menü de donuk kalır!
        Time.timeScale = 1f; 
        isPaused = false;
        SceneManager.LoadScene("MainMenu"); 
    }
}