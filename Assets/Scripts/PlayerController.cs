using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    [SerializeField] float moveSpeed = 0.05f;
    [SerializeField] float moveBaseheight = 0.5f;
    [SerializeField] float moveUpDown = 0.5f;

    [Header("�t�����ւ̎Q��")]
    [SerializeField] Camera mainCamera;
    [SerializeField] BlockHumanAnimatorController playerAnimator;

    [Header("����ւ̎Q��")]
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
        // ���͕���������
        //transform.LookAt(transform.position + dir);

        var basePos = transform.position + Vector3.up * moveBaseheight;
        var ray = new Ray(basePos, dir);

        // �i�s������raycast���Ĉړ������`�F�b�N
        RaycastHit hit, hitDown;
        // �i�s�����ɏ�Q�����Ȃ����Ƃ��m�F
        if (!Physics.Raycast(ray, out hit, moveSpeed))
        {
            var basePos2 = ray.origin + ray.direction * moveSpeed;
            //var downDistance = 1.5f;
            var downDistance = moveBaseheight + moveUpDown;
            var rayDown = new Ray(basePos2, -1 * Vector3.up * (basePos.y + moveUpDown));
            // �i�s�ꏊ�ɏ������邩
            if (Physics.Raycast(rayDown, out hitDown, downDistance))
            {
                // ���������ꏊ�Ɉړ�
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
