using UnityEngine;

public class DamageTesterMultiplayer : MonoBehaviour
{
    public PlayerHealthMultiplayer player1;
    public PlayerHealthMultiplayer player2;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            player1.TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            player2.TakeDamage(10);
        }
    }
}
