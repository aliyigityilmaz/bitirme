using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;

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

    void Update()
    {
        if (target == null)
        {
            Debug.LogWarning("ProjectileMover: Target is null.");
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < 0.1f)
        {
            Debug.Log("Projectile reached target. Disabling.");
            gameObject.SetActive(false);
        }
    }
}
