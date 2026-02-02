using UnityEngine;

public class DamageMask : MonoBehaviour
{
    public float damageIncreaseAmount = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.IncreaseDamage(damageIncreaseAmount);
                Destroy(gameObject); // يختفي الماسك
            }
        }
    }
}
