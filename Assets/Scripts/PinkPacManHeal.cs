using UnityEngine;

public class PinkPacManHeal : MonoBehaviour
{
    public float verilecekCan = 50f;
    public HologramMessage hologramScript; // Hologram scriptini buraya bağla
    private bool canVerildiMi = false; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !canVerildiMi)
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                playerHealth.Heal(verilecekCan);
                canVerildiMi = true;

                // Hologram mesajını tetikle
                if (hologramScript != null)
                {
                    hologramScript.MesajiGoster();
                }
            }
        }
    }
}