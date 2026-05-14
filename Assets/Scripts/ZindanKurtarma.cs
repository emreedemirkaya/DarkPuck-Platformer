using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class ZindanKurtarma : MonoBehaviour
{
    [Header("Referanslar")]
    public GameObject kurtarilanPinkPuck; 
    public GameObject thankYouYazisi; 
    public GameObject levelCompletePanel;
    public TextMeshProUGUI totalScoreText;

    [Header("Ses Yönetimi")]
    public AudioSource bossMusicSource; // BossMusicTrigger'da çalan müziğin olduğu kaynak
    public AudioSource levelCompleteAudioSource; // Zindandaki ses kaynağı
    public AudioClip levelCompleteSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ScoreManager üzerinden anahtar kontrolü
            if (ScoreManager.instance != null && ScoreManager.instance.anahtarVar)
            {
                // 1. BOSS MÜZİĞİNİ DURDUR (BossMusicTrigger'daki aynı objeyi hedef alıyoruz)
                if (bossMusicSource != null && bossMusicSource.isPlaying)
                {
                    bossMusicSource.Stop();
                    Debug.Log("Boss müziği durduruldu.");
                }

                // 2. LEVEL COMPLETE SESİNİ ÇAL
                if (levelCompleteAudioSource != null && levelCompleteSound != null)
                {
                    levelCompleteAudioSource.PlayOneShot(levelCompleteSound);
                }

                // 3. Yazıyı Aktif Et ve Takibi Başlat
                if (thankYouYazisi != null) thankYouYazisi.SetActive(true);

                if (kurtarilanPinkPuck != null)
                {
                    PinkPuckTakip takipScripti = kurtarilanPinkPuck.GetComponent<PinkPuckTakip>();
                    if (takipScripti != null) takipScripti.takipEt = true;
                }

                // 4. Zindan görsellerini kapat ve panel sürecini başlat
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                
                StartCoroutine(LevelCompleteSequence());
                ScoreManager.instance.anahtarVar = false;
            }
            else
            {
                Debug.Log("Anahtarın yok, zindan açılmadı.");
            }
        }
    }

    IEnumerator LevelCompleteSequence()
    {
        yield return new WaitForSeconds(3f);
        
        if (thankYouYazisi != null) thankYouYazisi.SetActive(false);
        if (levelCompletePanel != null) levelCompletePanel.SetActive(true);
        
        if (ScoreManager.instance != null && totalScoreText != null)
            totalScoreText.text = "Total Score: " + ScoreManager.instance.score.ToString();
    }

    // Buton Fonksiyonları
    public void AnaMenuyeDon() { SceneManager.LoadScene("MainMenuScene"); }
    public void OyundanCik() { Application.Quit(); }
}