using UnityEngine;

public class PlayerLightController : MonoBehaviour
{
    [Header("Işık Ayarları")]
    [Tooltip("Player'ın içine oluşturduğumuz Fener_Isigi objesini buraya sürükleyin")]
    public GameObject fenerIsigi;

    // Oyuncunun feneri alıp almadığını tutan gizli bir hafıza
    public bool hasLantern = false;

    void Update()
    {
        // Eğer oyuncu feneri aldıysa VE klavyeden 'F' tuşuna basarsa
        if (hasLantern && Input.GetKeyDown(KeyCode.F))
        {
            // Işığın mevcut durumunu tersine çevir (Kapalıysa aç, açıksa kapat)
            fenerIsigi.SetActive(!fenerIsigi.activeSelf);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eğer çarptığımız objenin etiketi "Lantern" ise
        if (other.CompareTag("Lantern"))
        {
            hasLantern = true; // Artık fenerimiz var!
            
            // Yerdeki fener objesini sahneden sil (sanki envantere almışız gibi)
            Destroy(other.gameObject);
            
            Debug.Log("Fener alındı! Tüneli aydınlatmak için 'F' tuşuna basın.");
        }
    }
}