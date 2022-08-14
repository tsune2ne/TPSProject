using UnityEngine;
using UnityEngine.VFX;

public class Gun : MonoBehaviour, IWeapon
{
    const string HitPointPrefabPath = "Spark/Spark";

    [SerializeField] GameObject Nozzle;

    const float MarginNextShot = 0.1f;
    float remainNextShot = 0f;

    void Update()
    {
        if (remainNextShot != 0f)
        {
            remainNextShot -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        if (remainNextShot > 0f) return;
        remainNextShot = MarginNextShot;

        // �m�Y���t���b�V��
        var resource = Resources.Load(HitPointPrefabPath);
        Instantiate(resource, Nozzle.transform.position, Quaternion.identity);

        // raycast���ē��������ꏊ�ɒ��e�G�t�F�N�g
        var ray = new Ray(Nozzle.transform.position, Nozzle.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Instantiate(resource, hit.point, Quaternion.Euler(0f, 180f, 0f));
        }

        // TODO raycast���đΏۂɓ����蔻��
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        var ray = new Ray(Nozzle.transform.position, Nozzle.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(ray.origin, hit.point - ray.origin);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(ray.origin, ray.direction);
        }
    }
#endif
}
