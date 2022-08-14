using UnityEngine;

/// <summary>
/// キャラに自動追従するカメラスクリプト
/// カメラアングルは手動操作
/// </summary>
[ExecuteInEditMode]
public class FollowCamera : MonoBehaviour
{
    [Header("カメラ設定")]
    [SerializeField] GameObject target;
    [SerializeField] float targetHeight = 2f;
    [SerializeField] float distance = 5f;
    [SerializeField] float cameraSpeed = 40f;

    const float MaxTopAngle = -30f, MaxLowAngle = 30f;
    [SerializeField, Range(MaxTopAngle, MaxLowAngle)] float topAngle;
    [SerializeField] float leftAngle;

    void Start()
    {
        // 開始時に被写体の後ろに設定
        leftAngle = -1 * Vector3.SignedAngle(target.transform.forward, Vector3.forward, Vector3.up) + 180f;
    }

    void Update()
    {
        var speed = Time.deltaTime * cameraSpeed;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            topAngle = Mathf.Max(topAngle - speed, MaxTopAngle);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            topAngle = Mathf.Min(topAngle + speed, MaxLowAngle);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            leftAngle = (leftAngle - speed) % 360;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            leftAngle = (leftAngle + speed) % 360;
        }
        UpdateCamera();
    }

    void UpdateCamera()
    {
        var center = target.transform.position + Vector3.up * targetHeight;
        var dir = Quaternion.Euler(topAngle, leftAngle, 0) * Vector3.forward;
        transform.position = center + distance * dir;
        transform.LookAt(center);

        // TODO 被写体からカメラにRayを飛ばす
        // TODO MAPにぶつかったらその手前にカメラを移動する
    }
}
