using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy target;
    private float damage;
    private float speed = 8f;

    private TrailRenderer trail;

    public void Init(Enemy targetEnemy, float dmg)
    {
        target = targetEnemy;
        damage = dmg;

        // Top görünümü
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr == null) mr = gameObject.AddComponent<MeshRenderer>();

        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf == null) mf = gameObject.AddComponent<MeshFilter>();

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mf.mesh = sphere.GetComponent<MeshFilter>().sharedMesh;
        Destroy(sphere);
        Destroy(GetComponent<Collider>());

        Material mat = new Material(Shader.Find("Standard"));
        mat.color = new Color(1.0f, 0.6f, 0.1f);
        mat.SetFloat("_Glossiness", 0.8f);
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", new Color(1.0f, 0.4f, 0.0f) * 1.5f);
        mr.material = mat;

        transform.localScale = Vector3.one * 0.15f;

        // İz efekti
        trail = gameObject.AddComponent<TrailRenderer>();
        trail.time          = 0.15f;
        trail.startWidth    = 0.12f;
        trail.endWidth      = 0.01f;
        trail.startColor    = new Color(1.0f, 0.5f, 0.1f, 0.8f);
        trail.endColor      = new Color(1.0f, 0.2f, 0.0f, 0.0f);
        trail.material      = new Material(Shader.Find("Sprites/Default"));

        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        if (target == null || target.CurrentState == EnemyState.Dead)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.transform.position + Vector3.up * 0.5f
                       - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        float dist = Vector3.Distance(transform.position,
                         target.transform.position + Vector3.up * 0.5f);
        if (dist < 0.25f)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
