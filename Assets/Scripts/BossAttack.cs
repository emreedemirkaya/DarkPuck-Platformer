using UnityEngine;
using System.Collections;
using System.Collections.Generic; // List kullanmak için

public class BossAttack : MonoBehaviour
{
    [Header("Yavru Ayarları")]
    public GameObject yavruPrefab;    // Oluşturulacak yavru prefab'ı
    public float saldırıAralığı = 4f; // Kaç saniyede bir yavru çıkacak?
    public Transform[] spawnNoktaları; // Yavruların çıkacağı noktalar (Ağız, kollar vb.)

    [Header("Menzil Ayarları")]
    public float saldırıMenzili = 15f; // Oyuncu bu kadar yakındaysa saldırsın
    private Transform player;

    private float zamanlayıcı;
    private bool saldırsınMı = false;

    void Start()
    {
        // Sahnedeki Player'ı bul
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        zamanlayıcı = saldırıAralığı; // İlk saldırı hemen başlasın diye
    }

    void Update()
    {
        if (player == null) return;

        // Oyuncu menzilde mi kontrol et
        float mesafe = Vector2.Distance(transform.position, player.position);
        if (mesafe <= saldırıMenzili)
        {
            zamanlayıcı += Time.deltaTime;

            if (zamanlayıcı >= saldırıAralığı)
            {
                zamanlayıcı = 0;
                Saldır();
            }
        }
    }

    void Saldır()
    {
        if (yavruPrefab == null || spawnNoktaları.Length == 0) return;

        // Rastgele bir çıkış noktası seç
        int rastgeleIndeks = Random.Range(0, spawnNoktaları.Length);
        Transform secilenNokta = spawnNoktaları[rastgeleIndeks];

        // Yavruyu o noktada oluştur
        Instantiate(yavruPrefab, secilenNokta.position, Quaternion.identity);

        Debug.Log("Boss yavru fırlattı!");
        
        // Buraya Boss'un fırlatma anı animasyonu veya sesi gelebilir
    }
}