using UnityEngine;

public class GateController : MonoBehaviour
{
    [Tooltip("Işığın kapıya ne kadar uzaktan etki edebileceği (Menzil)")]
    public float isikMenzili = 5f; 
    
    [Tooltip("Işık çekildikten KAÇ SANİYE SONRA kapının kapanacağı")]
    public float kapanmaGecikmesi = 0.5f; 
    
    private Collider2D gateCollider;
    private SpriteRenderer gateSprite;
    
    private Transform playerTransform;
    private PlayerLightController playerLight;
    private Color originalColor;

    // Geri sayım için kullanacağımız gizli kronometre
    private float kapanmaSayaci = 0f; 

    void Start()
    {
        gateCollider = GetComponent<Collider2D>();
        gateSprite = GetComponent<SpriteRenderer>();
        
        if(gateSprite != null)
        {
            originalColor = gateSprite.color;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if(playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerLight = playerObj.GetComponent<PlayerLightController>();
        }
    }

    void Update()
    {
        if (playerTransform != null && playerLight != null)
        {
            float mesafe = Vector2.Distance(transform.position, playerTransform.position);

            bool kapiyaBakiyor = false;
            if (playerTransform.localScale.x > 0 && transform.position.x > playerTransform.position.x)
                kapiyaBakiyor = true;
            else if (playerTransform.localScale.x < 0 && transform.position.x < playerTransform.position.x)
                kapiyaBakiyor = true;

            bool isikVuruyor = (mesafe <= isikMenzili) && playerLight.hasLantern && playerLight.fenerIsigi.activeInHierarchy && kapiyaBakiyor;

            if (isikVuruyor)
            {
                // IŞIK VURUYORSA: Kronometreyi hep tam dolu tut ve kapıyı aç
                kapanmaSayaci = kapanmaGecikmesi;
                
                gateCollider.enabled = false;
                if (gateSprite != null) gateSprite.color = new Color(0.5f, 0.8f, 1f, 0.4f);
            }
            else
            {
                // IŞIK GİTTİYSE: Kronometreyi geriye doğru saydırmaya başla
                if (kapanmaSayaci > 0)
                {
                    kapanmaSayaci -= Time.deltaTime; // Saniyeleri düşür
                }
                else // Kronometre SIFIRLANDIĞINDA kapıyı tamamen kapat
                {
                    gateCollider.enabled = true;
                    if (gateSprite != null) gateSprite.color = originalColor;
                }
            }
        }
    }
}