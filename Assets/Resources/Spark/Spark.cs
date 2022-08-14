using UnityEngine;
using UnityEngine.VFX;

public class Spark : MonoBehaviour
{
    [SerializeField] VisualEffect effect;

    const float DestroyTime = 1f;
    const float StopFireTime = 0.2f;
    float spanTime = 0f;

    private void Start()
    {
        effect.Play();
    }

    private void Update()
    {
        spanTime += Time.deltaTime;
        if (spanTime >= StopFireTime)
        {
            effect.Stop();
        }

        if (spanTime >= DestroyTime)
        {
            Destroy(gameObject);
        }
    }
}
