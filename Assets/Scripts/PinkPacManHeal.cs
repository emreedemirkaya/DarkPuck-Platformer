using UnityEngine;

public class PinkPacManHeal : MonoBehaviour
{
    [Header("Can Ayarları")]
    public float verilecekCan = 50f;
    
    // Oyuncu ekranda durdukça sürekli can almasın diye bir kontrol ekliyoruz
    private bool canVerildiMi = false; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Eğer çarpan obje "Player" ise ve daha önce can verilmediyse
        if (collision.CompareTag("Player") && !canVerildiMi)
        {
            // Oyuncunun üzerindeki PlayerHealth scriptine ulaş
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                // Can ekleme fonksiyonunu çağır
                playerHealth.Heal(verilecekCan);
                canVerildiMi = true; // Sadece 1 kere can versin

                Debug.Log("Pink Pac-Man'den 50 Can Alındı!");
                
                // İSTEĞE BAĞLI: Can aldıktan sonra ekranı/objeyi yok etmek istersen alttaki satırı açabilirsin:
                // Destroy(gameObject); 
            }
        }
    }
}