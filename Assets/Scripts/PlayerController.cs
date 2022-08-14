using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] float moveSpeed = 0.05f;
    [SerializeField] float moveBaseheight = 0.5f;
    [SerializeField] float moveUpDown = 0.5f;

    [Header("付属物への参照")]
    [SerializeField] Camera mainCamera;
    [SerializeField] BlockHumanAnimatorController playerAnimator;

    [Header("武器への参照")]
    [SerializeField] Gun gun;

    void Update()
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
        // 入力方向を向く
        //transform.LookAt(transform.position + dir);

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

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        var dir = transform.forward;
        var basePos = transform.position + Vector3.up * moveBaseheight;
        var ray = new Ray(basePos, dir);
        RaycastHit hit, hitDown;
        Vector3 hitPos;
        if (Physics.Raycast(ray, out hit, moveSpeed))
        {
            hitPos = hit.point;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(ray.origin, hitPos - ray.origin);
        }
        else
        {
            hitPos = ray.origin + ray.direction * moveSpeed * 20f;
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(ray.origin, hitPos - ray.origin);
        }

        Vector3 hitPos2;
        var downDistance = moveBaseheight + moveUpDown;
        var rayDown = new Ray(hitPos, -1 * Vector3.up * (basePos.y + moveUpDown));
        if (Physics.Raycast(rayDown, out hitDown, downDistance))
        {
            hitPos2 = hitDown.point;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(rayDown.origin, hitPos2 - rayDown.origin);
        }
        else
        {
            hitPos2 = rayDown.origin + rayDown.direction * downDistance;
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(rayDown.origin, hitPos2 - rayDown.origin);
        }

    }
#endif
}
