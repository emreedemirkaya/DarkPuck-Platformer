using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Tooltip("Oyuncunun ışınlanacağı hedef nokta")]
    public Transform destinationPoint;

    [Header("Kamera Ayarları")]
    public CameraController camController;
    public float newCameraYLevel = -10f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Çarpışan obje "Player" mı kontrol et
        if (other.CompareTag("Player"))
        {
            // 1. Oyuncuyu anında hedef noktaya ışınla
            other.transform.position = destinationPoint.position;

            // 2. Kamerayı yeni yeraltı seviyesine indir ve süzülmesini engelleyerek anında oraya oturt
            if (camController != null)
            {
                camController.currentYLevel = newCameraYLevel;
                camController.SnapCamera();
            }
        }
    }
}