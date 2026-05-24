using UnityEngine;

public class DefenseTower : MonoBehaviour
{
    [SerializeField] float attackRange = 5f;
    [SerializeField] float attackDamage = 15f;
    [SerializeField] float attackCooldown = 2f;

    Enemy currentTarget;
    float attackTimer;

    void Update()
    {
        attackTimer += Time.deltaTime;
        currentTarget = FindTargetInRange();

        if (currentTarget != null && attackTimer >= attackCooldown)
        {
            currentTarget.TakeDamage(attackDamage);
            attackTimer = 0f;
            Debug.Log($"Kule ateş etti: {attackDamage} hasar");
        }
    }

    Enemy FindTargetInRange()
    {
        Enemy[] allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Enemy nearest = null;
        float nearestDist = float.MaxValue;

        foreach (Enemy enemy in allEnemies)
        {
            if (enemy == null) continue;
            if (enemy.CurrentState == EnemyState.Dead) continue;

            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist <= attackRange && dist < nearestDist)
            {
                nearestDist = dist;
                nearest = enemy;
            }
        }

        return nearest;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
