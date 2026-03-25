using UnityEngine;

public class PersistentMusic : MonoBehaviour
{
    private static PersistentMusic instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahne değişse de bu objeyi silme!
        }
        else
        {
            Destroy(gameObject); // Eğer başka bir müzik objesi varsa onu sil
        }
    }
}