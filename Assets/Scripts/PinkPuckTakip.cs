using UnityEngine;

public class PinkPuckTakip : MonoBehaviour
{
    public Transform playerTransform; // Oyuncunun Transformu
    public float takipHizi = 5f;
    public Vector3 takipMesafesi = new Vector3(-1f, 1f, 0f); // Oyuncunun biraz sol üstünde dursun
    public bool takipEt = false;

    void Update()
    {
        if (takipEt && playerTransform != null)
        {
            // Oyuncunun hedef pozisyonunu hesapla
            Vector3 hedefPozisyon = playerTransform.position + takipMesafesi;
            
            // Yumuşak bir şekilde o pozisyona git (Lerp)
            transform.position = Vector3.Lerp(transform.position, hedefPozisyon, takipHizi * Time.deltaTime);
        }
    }
}