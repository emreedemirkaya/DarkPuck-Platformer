using UnityEngine;

public class KirilabilenZemin : MonoBehaviour
{
    // Oyuncu bu objeye fiziksel olarak çarptığında çalışır
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // İstersen buraya bir "Cam Kırılma" ses efekti ekleyebilirsin
            gameObject.SetActive(false); // Çizgiyi yok et, oyuncu içeri düşsün
        }
    }
}