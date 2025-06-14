using System.Collections.Generic;
using UnityEngine;

public class VFXActivator : MonoBehaviour
{
    public List<ParticleSystem> vfxList;
    public List<ParticleSystem> vfxOnEnemies;
    public List<ParticleSystem> projectile;
    public List<ParticleSystem> lookAt;
    public Transform projectileSpawnPoint;
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
        VFXActivator.instance.FireProjectile();
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
    public void FireProjectile()
    {
        if (projectile.Count == 0 || followTarget == null || projectileSpawnPoint == null) return;

        foreach (var proj in projectile)
        {
            proj.Stop();
            proj.transform.position = projectileSpawnPoint.position;
            proj.transform.rotation = Quaternion.LookRotation(followTarget.position - projectileSpawnPoint.position);

            ProjectileMover mover = proj.GetComponent<ProjectileMover>();
            if (mover != null)
            {
                mover.Init(followTarget);
            }

            proj.Play();
        }
    }
}
