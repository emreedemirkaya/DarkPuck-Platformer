using System.Collections;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    [Header("Fırlatma Ayarları")]
    public Vector2 firlatmaGucu = new Vector2(0f, 20f); 
    public float gerilmeSuresi = 0.3f; 
    public float ezilmeMiktari = 0.7f; 
    
    [Header("Algılama Ayarları")]
    public LayerMask playerKatmani; 
    public Vector2 kontrolKutuBoyutu = new Vector2(1.2f, 0.4f); 
    public float xOfset = 0f;
    public float yOfset = 0.8f;
    [Range(0.1f, 1.0f)]
    public float ustTemasHassasiyeti = 0.9f; 

    private bool isLaunching = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isLaunching)
        {
            //Çarpışma açısını kontrol et
            ContactPoint2D contact = collision.contacts[0];
            
            if (contact.normal.y < -ustTemasHassasiyeti) // Karakter yukardan aşağı bastığında normal aşağı bakar
            {
                Debug.Log("Onaylı Üst Temas: Ezilme tetikleniyor.");
                StartCoroutine(LaunchSequence(collision.gameObject));
            }
            else
            {
                Debug.Log("Yan temas engellendi. Normal Y: " + contact.normal.y);
            }
        }
    }

    private IEnumerator LaunchSequence(GameObject player)
    {
        isLaunching = true;
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        Vector3 originalScale = transform.localScale;
        
        // 1. EZİLME
        transform.localScale = new Vector3(originalScale.x, originalScale.y * ezilmeMiktari, originalScale.z);

        yield return new WaitForSeconds(gerilmeSuresi);

        // 2. ALAN KONTROLÜ (Hala üzerinde mi?)
        Vector2 kontrolMerkezi = (Vector2)transform.position + new Vector2(xOfset, yOfset);
        Collider2D hit = Physics2D.OverlapBox(kontrolMerkezi, kontrolKutuBoyutu, 0, playerKatmani);

        // 3. YAYI DÜZELT
        transform.localScale = originalScale;

        if (hit != null) 
        {
            playerRb.linearVelocity = Vector2.zero; 
            playerRb.AddForce(firlatmaGucu, ForceMode2D.Impulse);
            Debug.Log("Fırlatma Başarılı!");
        }

        yield return new WaitForSeconds(0.5f);
        isLaunching = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector2 kontrolMerkezi = (Vector2)transform.position + new Vector2(xOfset, yOfset);
        Gizmos.DrawWireCube(kontrolMerkezi, kontrolKutuBoyutu);
    }
}