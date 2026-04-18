using UnityEngine;

public class KirilabilenZemin : MonoBehaviour
{
    [Header("Ses Ayarları")]
    public AudioSource sesKaynagi;
    public AudioClip camKirilmaSesi;

    private bool kirildiMi = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Eğer çarpan Player ise ve henüz kırılmadıysa
        if (collision.gameObject.CompareTag("Player") && !kirildiMi)
        {
            KirilmaEfektiniBaslat();
        }
    }

    void KirilmaEfektiniBaslat()
    {
        kirildiMi = true;

        // 1. Sesi Çal
        if (sesKaynagi != null && camKirilmaSesi != null)
        {
            sesKaynagi.PlayOneShot(camKirilmaSesi);
        }

        // 2. Görselliği ve Çarpışmayı Kapat (Oyuncu içeri düşsün)
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // 3. Ses bittikten sonra objeyi tamamen yok et (Bellek temizliği)
        // Eğer ses 2 saniyeyse, 2 saniye sonra yok edilir.
        float sesSuresi = camKirilmaSesi != null ? camKirilmaSesi.length : 0.1f;
        Destroy(gameObject, sesSuresi);
    }
}