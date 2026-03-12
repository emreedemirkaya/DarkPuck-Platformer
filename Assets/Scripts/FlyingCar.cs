using UnityEngine;

public class FlyingCar : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    [Tooltip("Arabanın hızı. Sola gitmesi için eksi (-) değer girin.")]
    public float speed = 3f;

    [Header("Sınır Ayarları")]
    [Tooltip("Arabanın ekrandan çıkıp kaybolacağı X koordinatı")]
    public float destroyX = 30f;
    [Tooltip("Arabanın yeniden doğacağı (başa döneceği) X koordinatı")]
    public float spawnX = -30f;

    void Update()
    {
        // Arabayı belirtilen hızda sağa veya sola hareket ettir
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Araba sağa doğru uçuyorsa ve sınırı geçtiyse
        if (speed > 0 && transform.position.x > destroyX)
        {
            ResetPosition();
        }
        // Araba sola doğru uçuyorsa ve sınırı geçtiyse
        else if (speed < 0 && transform.position.x < destroyX)
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        // Arabayı başlangıç noktasına geri ışınla, yüksekliğini (Y) sabit tut
        transform.position = new Vector3(spawnX, transform.position.y, transform.position.z);
    }
}