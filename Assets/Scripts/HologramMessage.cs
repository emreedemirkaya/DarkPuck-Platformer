using UnityEngine;
using TMPro;
using System.Collections;

public class HologramMessage : MonoBehaviour
{
    public TextMeshProUGUI textElement;
    public float yazmaHizi = 0.05f;
    public string[] mesajlar;

    private void Start()
    {
        textElement.text = ""; // Başlangıçta boş
    }

    public void MesajiGoster()
    {
        StopAllCoroutines();
        // Rastgele bir mesaj seç
        string secilenMesaj = mesajlar[Random.Range(0, mesajlar.Length)];
        StartCoroutine(DaktiloYaz(secilenMesaj));
    }

    IEnumerator DaktiloYaz(string mesaj)
    {
        textElement.text = "";
        foreach (char harf in mesaj.ToCharArray())
        {
            textElement.text += harf;
            yield return new WaitForSeconds(yazmaHizi);
        }

        // 3 saniye sonra mesajın kaybolmasını istersen:
        yield return new WaitForSeconds(3f);
        textElement.text = "";
    }
}