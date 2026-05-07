using UnityEngine;

public class MagneticDoorManager : MonoBehaviour
{
    public static MagneticDoorManager Instance;

    [Header("Hedef Kapı")]
    public GameObject exitDoor; // Çıkış kapısı objesi

    [Header("Odadaki Şalterler")]
    public MagneticSwitch[] allSwitches; 

    void Awake()
    {
        Instance = this;
    }

    public void CheckAllSwitches()
    {
        foreach (MagneticSwitch s in allSwitches)
        {
            if (!s.isActivated) return; 
        }

        OpenDoor();
    }

    void OpenDoor()
    {
        Debug.Log("SİSTEM ERİŞİMİ ONAYLANDI: KAPI AÇILDI!");

        if (exitDoor != null)
        {
            // 1. Kapının Collider'ını Trigger yap (Artık içinden geçilebilir)
            Collider2D doorCollider = exitDoor.GetComponent<Collider2D>();
            if (doorCollider != null)
            {
                doorCollider.isTrigger = true;
            }

            // 2. Görsel Geri Bildirim: Kapıyı yarı şeffaf yap
            // (Böylece oyuncu kapının katı olmadığını anlar)
            SpriteRenderer doorSR = exitDoor.GetComponent<SpriteRenderer>();
            if (doorSR != null)
            {
                Color c = doorSR.color;
                c.a = 0.4f; // %40 şeffaflık
                doorSR.color = c;
            }
            
            // Eğer kapıda bir ışık veya neon varsa onları da buradan değiştirebilirsin
        }
    }
}