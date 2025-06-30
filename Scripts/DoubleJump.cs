using UnityEngine; 
public class DoubleJump : MonoBehaviour
{
    public float duration = 0f; // 0 = permanente
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerMovement>()?
               .EnableDoubleJump(duration);
            Destroy(gameObject);
        }
    }
}
