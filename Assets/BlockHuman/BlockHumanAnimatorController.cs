using UnityEngine;

/// <summary>
/// BlockHuman��Animator�Ăяo�������̏W��N���X
/// </summary>
public class BlockHumanAnimatorController : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void Run()
    {
        animator.SetBool("isRun", true);
    }

    public void RunEnd()
    {
        animator.SetBool("isRun", false);
    }

    public void Run(float front, float left)
    {
        animator.SetFloat("Run.Front", front);
        animator.SetFloat("Run.Left", left);
    }


    public void Jump()
    {
        animator.SetBool("isJump", true);
    }

    public void JumpEnd()
    {
        animator.SetBool("isJump", false);
    }

    public void GunLookAt(Vector3 dir)
    {
        // TODO ���ʂ��猩�č���1�A�E��-1
        animator.SetFloat("DirectionalBlend", 0.5f);
    }

    public void GunShot()
    {
        animator.SetTrigger("Gun.ShotTrigger");
    }
}
