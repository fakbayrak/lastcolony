using UnityEngine;

public class DefenseTower : MonoBehaviour
{
    [SerializeField] float attackRange   = 7f;   // 5 → 7
    [SerializeField] float attackDamage  = 25f;  // 15 → 25
    [SerializeField] float attackCooldown = 1.2f; // 2 → 1.2

    Enemy currentTarget;
    float attackTimer;

    private float baseAttackDamage;
    private bool bonusActive = false;

    private void Start()
    {
        baseAttackDamage = attackDamage;

        if (GetComponent<TowerVisual>() == null)
            gameObject.AddComponent<TowerVisual>();
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
        currentTarget = FindTargetInRange();

        if (currentTarget != null && attackTimer >= attackCooldown)
        {
            FireProjectile(currentTarget);
            attackTimer = 0f;
        }
    }

    void FireProjectile(Enemy target)
    {
        // Ateş noktası: kulenin üst kısmı
        Vector3 firePoint = transform.position + Vector3.up * 1.6f;

        GameObject projObj = new GameObject("Projectile");
        projObj.transform.position = firePoint;

        Projectile proj = projObj.AddComponent<Projectile>();
        proj.Init(target, attackDamage);
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

    public void ApplyNPCBonus()
    {
        if (!bonusActive)
        {
            attackDamage = baseAttackDamage + 10f;
            bonusActive = true;
            Debug.Log("[DefenseTower] NPC bonusu aktif: saldırı gücü +" + 10f);
        }
    }

    public void RemoveNPCBonus()
    {
        attackDamage = baseAttackDamage;
        bonusActive = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
