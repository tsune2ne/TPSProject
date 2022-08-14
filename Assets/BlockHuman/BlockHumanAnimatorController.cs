using UnityEngine;

/// <summary>
/// BlockHumanのAnimator呼び出し処理の集約クラス
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
        // TODO 正面から見て左が1、右が-1
        animator.SetFloat("DirectionalBlend", 0.5f);
    }

    public void GunShot()
    {
        animator.SetTrigger("Gun.ShotTrigger");
    }
}
