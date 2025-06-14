using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;

    public void Init(Transform target)
    {
        this.target = target;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < 0.1f)
        {
            gameObject.SetActive(false);
        }
    }
}
