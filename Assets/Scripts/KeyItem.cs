using UnityEngine;

public class KeyItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Eğer çarpan obje Player ise
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            
            if (player != null)
            {
                // Oyuncunun cebine anahtarı koy
                player.hasKey = true;
                Debug.Log("Anahtar alındı! Çıkış kapısı açılabilir.");
                
                // Anahtar objesini sahneden yok et
                Destroy(gameObject);
            }
        }
    }
}