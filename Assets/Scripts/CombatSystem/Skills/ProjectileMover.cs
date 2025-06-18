using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;
    private bool hasStartedMoving = false;
    void Start()
    {
        if (VFXActivator.instance != null)
        {
            target = VFXActivator.instance.followTarget;
            Debug.Log($"[ProjectileMover] Start() - Assigned target: {(target != null ? target.name : "NULL")}");
        }
        else
        {
            Debug.LogWarning("[ProjectileMover] VFXActivator.instance null!");
        }
    }
    public void Init()
    {

    }
    void OnEnable()
    {
        hasStartedMoving = false;
        if (VFXActivator.instance != null)
        {
            target = VFXActivator.instance.followTarget;

            Transform spawnPoint = FindSpawnPointForThisProjectile();

            if (spawnPoint != null)
            {
                transform.position = spawnPoint.position;
                transform.rotation = Quaternion.LookRotation(target.position - spawnPoint.position);
            }
            hasStartedMoving = true;
        }

    }

    void Update()
    {
        if (!hasStartedMoving || target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < 0.1f)
            {
                hasStartedMoving = true;
            }
        }

    }

    Transform FindSpawnPointForThisProjectile()
    {
        string name = gameObject.name;

        if (name.Contains("Aeliana"))
            return VFXActivator.instance.projectileSpawnPointForAeliana;
        if (name.Contains("Velora"))
            return VFXActivator.instance.projectileSpawnPointForVelora;
        if (name.Contains("Kaelion"))
            return VFXActivator.instance.projectileSpawnPointForKaelion;

        EnemyTargetable[] enemies = GameObject.FindObjectsOfType<EnemyTargetable>();
        foreach (var enemy in enemies)
        {
            if (enemy.name.Contains("Rifler") && name.Contains("Enemy1"))
                return enemy.assignedSpawnPoint;
            if (enemy.name.Contains("Magic") && name.Contains("Enemy2"))
                return enemy.assignedSpawnPoint;
        }

        return null;
    }
}
