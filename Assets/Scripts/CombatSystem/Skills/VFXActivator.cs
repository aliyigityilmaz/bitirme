using System.Collections.Generic;
using UnityEngine;

public class VFXActivator : MonoBehaviour
{
    public List<ParticleSystem> vfxList;
    public List<ParticleSystem> vfxOnEnemies;
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
    }
    void OnEnable()
    {        
        PlayActiveVFX();
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
}
