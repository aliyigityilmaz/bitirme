using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXActivator : MonoBehaviour
{
    public List<ParticleSystem> vfxList;
    public List<ParticleSystem> vfxOnEnemies;
    public List<ParticleSystem> projectileForAeliana;
    public List<ParticleSystem> projectileForVelora;
    public List<ParticleSystem> projectileForKaelion;
    public List<ParticleSystem> projectileForEnemy1;
    public List<ParticleSystem> projectileForEnemy2;
    public List<ParticleSystem> lookAt;
    public Transform projectileSpawnPointForAeliana;
    public Transform projectileSpawnPointForVelora;
    public Transform projectileSpawnPointForKaelion;
    public List<Transform> projectileSpawnPointsForEnemies;
    public Transform followTarget;
    public static VFXActivator instance;

    void Awake()
    {
        instance = this;
        if (followTarget == null)
        {
            GameObject ft = GameObject.Find("FollowTarget");
            if (ft != null)
            {
                followTarget = ft.transform;
                Debug.Log($"[VFXActivator] Found and assigned FollowTarget: {followTarget.name}");
            }
            else
            {
                Debug.LogWarning("VFXActivator: followTarget bulunamadý.");
            }
        }
    }
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        AutoPopulateVFXLists();


    }
    void AutoPopulateVFXLists()
    {
        vfxList = new List<ParticleSystem>();
        vfxOnEnemies = new List<ParticleSystem>();
        projectileForAeliana = new List<ParticleSystem>();
        projectileForVelora = new List<ParticleSystem>();
        projectileForKaelion = new List<ParticleSystem>();
        projectileForEnemy1 = new List<ParticleSystem>();
        projectileForEnemy2 = new List<ParticleSystem>();
        lookAt = new List<ParticleSystem>();
        projectileSpawnPointsForEnemies = new List<Transform>();

        // Enemy spawn pointlerini tag ya da isim bazlý bul
        foreach (var t in FindObjectsOfType<Transform>())
        {
            if (t.CompareTag("EnemyProjectileSpawn"))
            {
                projectileSpawnPointsForEnemies.Add(t);
            }
        }

        VFXIdentifier[] vfxIdentifiers = FindObjectsOfType<VFXIdentifier>();

        foreach (var identifier in vfxIdentifiers)
        {
            var ps = identifier.GetComponent<ParticleSystem>();
            if (ps == null) continue;

            vfxList.Add(ps);

            switch (identifier.type)
            {
                case VFXType.OnEnemy:
                    vfxOnEnemies.Add(ps);
                    break;
                case VFXType.AelianaProjectile:
                    projectileForAeliana.Add(ps);
                    break;
                case VFXType.VeloraProjectile:
                    projectileForVelora.Add(ps);
                    break;
                case VFXType.KaelionProjectile:
                    projectileForKaelion.Add(ps);
                    break;
                case VFXType.Enemy1Projectile:
                    projectileForEnemy1.Add(ps);
                    break;
                case VFXType.Enemy2Projectile:
                    projectileForEnemy2.Add(ps);
                    break;
                case VFXType.LookAt:
                    lookAt.Add(ps);
                    break;
            }
        }
    }
    private void Update()
    {
        if (followTarget != null)
        {
            foreach (var vfx in vfxOnEnemies)
            {
                vfx.transform.position = new Vector3(followTarget.transform.position.x, 2.2f, followTarget.transform.position.z);
            }
        }
        foreach (var vfx in lookAt)
        {
            vfx.transform.LookAt(followTarget.transform);
        }
    }
    void OnEnable()
    {        
        PlayActiveVFX();
        VFXActivator.instance.FireProjectileForAeliana();
        VFXActivator.instance.FireProjectileForVelora();
        VFXActivator.instance.FireProjectileForKaelion();
        FireProjectilesForEnemies();
    }

    public void PlayActiveVFX()
    {
        foreach (var vfx in vfxList)
        {
            if (vfx != null && vfx.gameObject.activeInHierarchy)
            {
                vfx.Play();
            }
        }
    }
    public void FireProjectileForAeliana()
    {
        if (projectileForAeliana.Count == 0 || followTarget == null || projectileSpawnPointForAeliana == null) return;

        foreach (var proj in projectileForAeliana)
        {
            proj.Stop();
            proj.transform.position = projectileSpawnPointForAeliana.position;
            proj.transform.rotation = Quaternion.LookRotation(followTarget.position - projectileSpawnPointForAeliana.position);

            ProjectileMover mover = proj.GetComponent<ProjectileMover>();


            proj.Play();
        }
    }
    public void FireProjectileForVelora()
    {
        if (projectileForVelora.Count == 0 || followTarget == null || projectileSpawnPointForVelora == null) return;

        foreach (var proj in projectileForVelora)
        {
            proj.Stop();
            proj.transform.position = projectileSpawnPointForVelora.position;
            proj.transform.rotation = Quaternion.LookRotation(followTarget.position - projectileSpawnPointForVelora.position);

            ProjectileMover mover = proj.GetComponent<ProjectileMover>();


            proj.Play();
        }
    }
    public void FireProjectileForKaelion()
    {
        if (projectileForKaelion.Count == 0 || followTarget == null || projectileSpawnPointForKaelion == null) return;

        foreach (var proj in projectileForKaelion)
        {
            proj.Stop();
            proj.transform.position = projectileSpawnPointForKaelion.position;
            proj.transform.rotation = Quaternion.LookRotation(followTarget.position - projectileSpawnPointForKaelion.position);

            ProjectileMover mover = proj.GetComponent<ProjectileMover>();


            proj.Play();
        }
    }
    public void FireProjectilesForEnemies()
    {
        Debug.Log("Firing enemy projectiles...");
        if (followTarget == null)
        {
            Debug.LogWarning("FollowTarget is null in FireProjectilesForEnemies!");
            return;
        }
        var enemies = FindObjectsOfType<EnemyTargetable>();

        foreach (var enemy in enemies)
        {
            if (enemy.assignedSpawnPoint == null)
            {
                Debug.LogWarning($"{enemy.name} has no spawn point assigned.");
                continue;
            }

            List<ParticleSystem> projectiles = null;
            if (enemy.name.Contains("Rifler Enemy Combat"))
                projectiles = projectileForEnemy1;
            else if (enemy.name.Contains("Magic Enemy Combat"))
                projectiles = projectileForEnemy2;

            if (projectiles == null || projectiles.Count == 0)
            {
                Debug.LogWarning($"No projectiles found for enemy: {enemy.name}");
                continue;
            }

            foreach (var proj in projectiles)
            {
                if (proj == null)
                {
                    Debug.LogWarning("Projectile is null in list");
                    continue;
                }

                proj.Stop();
                proj.transform.position = enemy.assignedSpawnPoint.position;
                proj.transform.rotation = Quaternion.LookRotation(followTarget.position - enemy.assignedSpawnPoint.position);

                ProjectileMover mover = proj.GetComponent<ProjectileMover>();


                proj.gameObject.SetActive(true);
                proj.Play();
            }
        }
    }

}
