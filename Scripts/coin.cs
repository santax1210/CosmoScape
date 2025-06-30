using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && HUDManager.instance != null)
        {
            HUDManager.instance.AddCoin(coinValue);
            Destroy(gameObject);
        }
    }
}

