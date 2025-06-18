using UnityEngine;
public enum VFXType
{
    General,
    OnEnemy,
    AelianaProjectile,
    VeloraProjectile,
    KaelionProjectile,
    Enemy1Projectile,
    Enemy2Projectile,
    LookAt
}
public class VFXIdentifier : MonoBehaviour
{
    public VFXType type;
    private void OnEnable()
    {
        RegisterToVFXActivator();

        var ps = GetComponent<ParticleSystem>();
        if (ps != null && type.ToString().Contains("Projectile"))
        {
            Transform spawnPoint = GetSpawnPoint();
            if (spawnPoint != null)
            {
                ps.transform.position = spawnPoint.position;
                ps.transform.rotation = Quaternion.LookRotation(VFXActivator.instance.followTarget.position - spawnPoint.position);
            }
        }
    }

    private void OnDisable()
    {
        UnregisterFromVFXActivator();
    }

    void RegisterToVFXActivator()
    {
        if (VFXActivator.instance == null) return;

        var ps = GetComponent<ParticleSystem>();
        if (ps == null) return;

        VFXActivator activator = VFXActivator.instance;

        if (!activator.vfxList.Contains(ps))
            activator.vfxList.Add(ps);

        switch (type)
        {
            case VFXType.OnEnemy:
                if (!activator.vfxOnEnemies.Contains(ps))
                    activator.vfxOnEnemies.Add(ps);
                break;
            case VFXType.AelianaProjectile:
                if (!activator.projectileForAeliana.Contains(ps))
                    activator.projectileForAeliana.Add(ps);
                break;
            case VFXType.VeloraProjectile:
                if (!activator.projectileForVelora.Contains(ps))
                    activator.projectileForVelora.Add(ps);
                break;
            case VFXType.KaelionProjectile:
                if (!activator.projectileForKaelion.Contains(ps))
                    activator.projectileForKaelion.Add(ps);
                break;
            case VFXType.Enemy1Projectile:
                if (!activator.projectileForEnemy1.Contains(ps))
                    activator.projectileForEnemy1.Add(ps);
                break;
            case VFXType.Enemy2Projectile:
                if (!activator.projectileForEnemy2.Contains(ps))
                    activator.projectileForEnemy2.Add(ps);
                break;
            case VFXType.LookAt:
                if (!activator.lookAt.Contains(ps))
                    activator.lookAt.Add(ps);
                break;
        }
    }

    void UnregisterFromVFXActivator()
    {
        if (VFXActivator.instance == null) return;

        var ps = GetComponent<ParticleSystem>();
        if (ps == null) return;

        VFXActivator.instance.vfxList.Remove(ps);
        VFXActivator.instance.vfxOnEnemies.Remove(ps);
        VFXActivator.instance.projectileForAeliana.Remove(ps);
        VFXActivator.instance.projectileForVelora.Remove(ps);
        VFXActivator.instance.projectileForKaelion.Remove(ps);
        VFXActivator.instance.projectileForEnemy1.Remove(ps);
        VFXActivator.instance.projectileForEnemy2.Remove(ps);
        VFXActivator.instance.lookAt.Remove(ps);
    }
    Transform GetSpawnPoint()
    {
        if (VFXActivator.instance == null) return null;

        switch (type)
        {
            case VFXType.AelianaProjectile:
                return VFXActivator.instance.projectileSpawnPointForAeliana;
            case VFXType.VeloraProjectile:
                return VFXActivator.instance.projectileSpawnPointForVelora;
            case VFXType.KaelionProjectile:
                return VFXActivator.instance.projectileSpawnPointForKaelion;
            case VFXType.Enemy1Projectile:
            case VFXType.Enemy2Projectile:
                var enemy = GetComponentInParent<EnemyTargetable>();
                return enemy != null ? enemy.assignedSpawnPoint : null;
            default:
                return null;
        }
    }
}
