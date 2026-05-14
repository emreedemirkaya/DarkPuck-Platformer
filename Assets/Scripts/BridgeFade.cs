using UnityEngine;
using System.Collections;

public class BridgeFade : MonoBehaviour
{
    public void KopruyuBelirt()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Collider2D col = GetComponent<Collider2D>();
        float alpha = 0;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * 0.5f; // Buradaki 0.5f hızı belirler
            sr.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        if (col != null) col.enabled = true; // Tamamen görünür olunca fizik açılır
    }
}