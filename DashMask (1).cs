using UnityEngine;

public class DashMask : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // التحقق من التاج "Player"
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.EnableDash();
                Destroy(gameObject); // يختفي الماسك
            }
        }
    }
}
