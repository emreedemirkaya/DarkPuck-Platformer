using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Sahnede "Dot" etiketine sahip kaç obje kaldığını sayıyoruz
            GameObject[] remainingDots = GameObject.FindGameObjectsWithTag("Dot");

            if (remainingDots.Length == 0)
            {
                Debug.Log("Tüm veriler toplandı! Level Complete!");
                
                // LevelCompleteManager'ı bul ve çalıştır
                LevelCompleteManager manager = FindFirstObjectByType<LevelCompleteManager>();
                if (manager != null && ScoreManager.instance != null)
                {
                    manager.ShowLevelComplete(ScoreManager.instance.score);
                }
            }
            else
            {
                // Eğer sahnede hala Dot varsa oyuncuyu uyar
                Debug.Log("SİSTEM UYARISI: Hala toplanmamış " + remainingDots.Length + " adet veri (Dot) var! Geri dön!");
            }
        }
    }
}