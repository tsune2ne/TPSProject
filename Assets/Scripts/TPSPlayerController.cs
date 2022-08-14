using UnityEngine;

[ExecuteInEditMode]
public class TPSPlayerController : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    [SerializeField] float moveSpeed = 0.01f;
    [SerializeField] float moveBaseheight = 0.3f;
    [SerializeField] float moveUpDown = 0.3f;

    [Header("�J�����ݒ�")]
    [SerializeField] Camera mainCamera;
    [SerializeField] Vector3 cameraCenter = new Vector3(0f, 0.6f, 0f);
    [SerializeField] float distance = 1f;
    [SerializeField] float cameraSpeed = 40f;
    const float MaxTopAngle = -30f, MaxLowAngle = 30f;
    [SerializeField, Range(MaxTopAngle, MaxLowAngle)] float topAngle = 0f;
    [SerializeField] float leftAngle = 180f;

    [Header("�t�����ւ̎Q��")]
    [SerializeField] BlockHumanAnimatorController playerAnimator;
    [SerializeField] Gun gun;

    private void Update()
    {
        UpdateCamera();
        UpdateMove();
    }

    void UpdateCamera()
    {
        // TODO �J����������ɁB�^�񒆂��^�Q�ɂȂ�
        // TODO �J�[�\���ړ��E�\���L�[�ŃJ�����ړ�
        // TODO �J�����̐悪�L�����̕����ɂ���

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

        // �L�����͓��͕����Ɣ��΂�����
        var charaDir = Quaternion.Euler(0, leftAngle, 0) * Vector3.forward;
        transform.LookAt(transform.position - charaDir);


        // TODO ��ʑ̂���J������Ray���΂�
        // TODO MAP�ɂԂ������炻�̎�O�ɃJ�������ړ�����
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
}
