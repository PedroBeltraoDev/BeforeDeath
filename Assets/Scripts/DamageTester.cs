using UnityEngine;

public class DamageTester : MonoBehaviour
{
    public PlayerHealth playerHealth;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerHealth.TakeDamage(10);
        }
    }
}
