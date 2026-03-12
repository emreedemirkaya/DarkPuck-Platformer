using UnityEngine;

public class SawRotator : MonoBehaviour
{
    [Tooltip("Testerenin dönüş hızı. Artı değerler bir yöne, eksi değerler diğer yöne döndürür.")]
    public float spinSpeed = 300f;

    void Update()
    {
        // Testereyi Z ekseninde (kendi etrafında) sürekli döndür
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }
}