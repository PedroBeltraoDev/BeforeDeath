using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;
    public float detectionRange = 5f;
    public Transform player;

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }
}
