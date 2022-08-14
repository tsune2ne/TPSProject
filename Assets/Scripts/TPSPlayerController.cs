using UnityEngine;

[ExecuteInEditMode]
public class TPSPlayerController : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] float moveSpeed = 0.01f;
    [SerializeField] float moveBaseheight = 0.3f;
    [SerializeField] float moveUpDown = 0.3f;

    [Header("カメラ設定")]
    [SerializeField] Camera mainCamera;
    [SerializeField] Vector3 cameraCenter = new Vector3(0f, 0.6f, 0f);
    [SerializeField] float distance = 1f;
    [SerializeField] float cameraSpeed = 40f;
    const float MaxTopAngle = -30f, MaxLowAngle = 30f;
    [SerializeField, Range(MaxTopAngle, MaxLowAngle)] float topAngle = 0f;
    [SerializeField] float leftAngle = 180f;

    [Header("付属物への参照")]
    [SerializeField] BlockHumanAnimatorController playerAnimator;
    [SerializeField] Gun gun;

    private void Update()
    {
        UpdateCamera();
        UpdateMove();
    }

    void UpdateCamera()
    {
        // TODO カメラを後方に。真ん中がタゲになる
        // TODO カーソル移動・十字キーでカメラ移動
        // TODO カメラの先がキャラの方向にする

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

        var center = transform.position + cameraCenter;
        var dir = Quaternion.Euler(topAngle, leftAngle, 0) * Vector3.forward;
        mainCamera.transform.position = center + distance * dir;
        mainCamera.transform.LookAt(center);

        // キャラは入力方向と反対を向く
        var charaDir = Quaternion.Euler(0, leftAngle, 0) * Vector3.forward;
        transform.LookAt(transform.position - charaDir);


        // TODO 被写体からカメラにRayを飛ばす
        // TODO MAPにぶつかったらその手前にカメラを移動する
    }

    void UpdateMove()
    {
        var forwardDir = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z);
        var rightDir = new Vector3(mainCamera.transform.right.x, 0, mainCamera.transform.right.z);
        if (Input.GetKey(KeyCode.W))
        {
            playerAnimator.Run(1f, 0f);
            Move(forwardDir);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            playerAnimator.Run(-1f, 0f);
            Move(-forwardDir);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerAnimator.Run(0f, 1f);
            Move(rightDir);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            playerAnimator.Run(0f, -1f);
            Move(-rightDir);
        }
        else
        {
            playerAnimator.Run(0f, 0f);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            playerAnimator.Jump();
        }
        else
        {
            playerAnimator.JumpEnd();
        }

        if (Input.GetMouseButton(0))
        {
            playerAnimator.GunShot();
            gun.Attack();
        }
    }

    void Move(Vector3 dir)
    {
        var basePos = transform.position + Vector3.up * moveBaseheight;
        var ray = new Ray(basePos, dir);

        // 進行方向にraycastして移動距離チェック
        RaycastHit hit, hitDown;
        // 進行方向に障害物がないことを確認
        if (!Physics.Raycast(ray, out hit, moveSpeed))
        {
            var basePos2 = ray.origin + ray.direction * moveSpeed;
            //var downDistance = 1.5f;
            var downDistance = moveBaseheight + moveUpDown;
            var rayDown = new Ray(basePos2, -1 * Vector3.up * (basePos.y + moveUpDown));
            // 進行場所に床があるか
            if (Physics.Raycast(rayDown, out hitDown, downDistance))
            {
                // 当たった場所に移動
                transform.position = hitDown.point;
            }
        }
    }
}
