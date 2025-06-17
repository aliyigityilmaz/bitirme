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
    public Transform projectileSpawnPointForEnemy1;
    public Transform projectileSpawnPointForEnemy2;
    public Transform followTarget;
    public static VFXActivator instance;

    void Awake()
    {
        instance = this;
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
        VFXActivator.instance.FireProjectileForEnemy1();
        VFXActivator.instance.FireProjectileForEnemy2();
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
            if (mover != null)
            {
                mover.Init(followTarget);
            }

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
            if (mover != null)
            {
                mover.Init(followTarget);
            }

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
            if (mover != null)
            {
                mover.Init(followTarget);
            }

            proj.Play();
        }
    }
    public void FireProjectileForEnemy1()
    {
        if (projectileForEnemy1.Count == 0 || followTarget == null || projectileSpawnPointForEnemy1 == null) return;

        foreach (var proj in projectileForEnemy1)
        {
            proj.Stop();
            proj.transform.position = projectileSpawnPointForEnemy1.position;
            proj.transform.rotation = Quaternion.LookRotation(followTarget.position - projectileSpawnPointForEnemy1.position);

            ProjectileMover mover = proj.GetComponent<ProjectileMover>();
            if (mover != null)
            {
                mover.Init(followTarget);
            }

            proj.Play();
        }
    }
    public void FireProjectileForEnemy2()
    {
        if (projectileForEnemy2.Count == 0 || followTarget == null || projectileSpawnPointForEnemy2 == null) return;

        foreach (var proj in projectileForEnemy2)
        {
            proj.Stop();
            proj.transform.position = projectileSpawnPointForEnemy2.position;
            proj.transform.rotation = Quaternion.LookRotation(followTarget.position - projectileSpawnPointForEnemy2.position);

            ProjectileMover mover = proj.GetComponent<ProjectileMover>();
            if (mover != null)
            {
                mover.Init(followTarget);
            }

            proj.Play();
        }
    }
}
