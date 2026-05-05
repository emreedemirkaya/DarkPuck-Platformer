using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Elemanları")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    [Header("Ses Efektleri")]
    public AudioSource gameOverSesi; // YENİ EKLENDİ: Game Over sesini çalacak bileşen

    public void ShowGameOver(int totalScore)
    {
        gameOverPanel.SetActive(true); 
        finalScoreText.text = "TOTAL SKOR: " + totalScore.ToString(); 
        
        Time.timeScale = 0f; 

        // 1. ARKA PLAN MÜZİĞİNİ DURDUR
        GameObject musicManagerObj = GameObject.Find("MusicManager");
        if (musicManagerObj != null)
        {
            AudioSource arkaPlanMuzigi = musicManagerObj.GetComponent<AudioSource>();
            if (arkaPlanMuzigi != null) arkaPlanMuzigi.Stop(); 
        }

        // 2. ALARM SESİNİ DURDUR 
        GameObject alarmObjesi = GameObject.Find("SecurityCamera");
        if (alarmObjesi != null)
        {
            AudioSource alarmSesi = alarmObjesi.GetComponent<AudioSource>();
            if (alarmSesi != null) alarmSesi.Stop();
        }

        // 3. GAME OVER SESİNİ ÇAL (YENİ EKLENDİ)
        if (gameOverSesi != null)
        {
            gameOverSesi.Play();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        
        MuzigiTekrarBaslat();

        // 1. SKORU TAMAMEN SIFIRLA
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.score = 0;
            // Eğer can sistemin ScoreManager içindeyse, baştan başlarken canı da fullüyoruz:
        }

        // 2. HANGİ LEVELDE ÖLÜRSE ÖLSÜN LEVEL 1'E DÖN!
        SceneManager.LoadScene("Level1"); 
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        MuzigiTekrarBaslat();
        
        // Ana menüye dönerken de skoru sıfırla
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.score = 0;
        }

        SceneManager.LoadScene("MainMenu");
    }

    private void MuzigiTekrarBaslat()
    {
        GameObject musicManagerObj = GameObject.Find("MusicManager");
        if (musicManagerObj != null)
        {
            AudioSource arkaPlanMuzigi = musicManagerObj.GetComponent<AudioSource>();
            
            if (arkaPlanMuzigi != null && !arkaPlanMuzigi.isPlaying)
            {
                arkaPlanMuzigi.Play(); 
            }
        }
    }
}