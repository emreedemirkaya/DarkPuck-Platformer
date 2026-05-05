using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("Veriler")]
    public int score = 0;
    public float mevcutCan = 100f; // Canı burada tutuyoruz

    [Header("UI Referansları")]
    public TextMeshProUGUI scoreText;
    public int levelBaslangicSkoru = 0;
    private void Awake()
    {
        // Singleton Yapısı
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahneler arası bu objeyi koru
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    private void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Her yeni sahne yüklendiğinde UI'yı bul ve güncelle
        GameObject go = GameObject.Find("ScoreText");
        if (go != null) scoreText = go.GetComponent<TextMeshProUGUI>();
        
        UpdateScoreUI();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = "SCORE:" + score;
    }
}