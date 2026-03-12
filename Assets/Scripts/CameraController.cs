using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Takip Ayarları")]
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    public bool followYAxis = false; // Zıplamayı takip etsin mi?

    [Header("Harita Sınırları (Clamping)")]
    public bool useBounds = false;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    [Header("Yeraltı/Kat Ayarları")]
    // Eğer followYAxis false ise, kameranın sabit kalacağı Y seviyesi.
    // Normalde 0'dır, yeraltına inince bunu eksi bir değere çekeceğiz.
    public float currentYLevel = 0f; 

    void LateUpdate()
    {
        if (target == null) return;

        // followYAxis true ise karakteri takip et, false ise currentYLevel hizasında kal
        float targetY = followYAxis ? target.position.y : currentYLevel;
        
        Vector3 desiredPosition = new Vector3(target.position.x, targetY, 0) + offset;

        if (useBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    // Işınlanma sırasında kamerayı süzülmeden ANINDA yeni yere götürmek için
    public void SnapCamera()
    {
        if (target == null) return;
        
        float targetY = followYAxis ? target.position.y : currentYLevel;
        Vector3 desiredPosition = new Vector3(target.position.x, targetY, 0) + offset;

        if (useBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
        }

        transform.position = desiredPosition;
    }
}