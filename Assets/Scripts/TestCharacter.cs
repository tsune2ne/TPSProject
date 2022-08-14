using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacter : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] Content[] contents;

    [SerializeField, Range(-1f, 1f)] float brend;

    [System.Serializable]
    public class Content
    {
        public enum ParamType
        {
            Float,
            Int,
            Bool,
            Trigger,
        }

        public string animName;
        public ParamType paramType;
        public KeyCode keyCode;
        public string value;

        public Content(string targetAnimName, ParamType paramType, KeyCode keyCode, string value)
        {
            this.animName = targetAnimName;
            this.paramType = paramType;
            this.keyCode = keyCode;
            this.value = value;
        }
    }


    void Update()
    {
        foreach (var content in contents)
        {
            switch (content.paramType)
            {
                case Content.ParamType.Float:
                    if (Input.GetKey(content.keyCode))
                    {
                        Debug.LogError(float.Parse(content.value));
                        animator.SetFloat(content.animName, float.Parse(content.value));
                    }
                    else
                        animator.SetFloat(content.animName, 0f);
                    break;
                case Content.ParamType.Int:
                    if (Input.GetKey(content.keyCode))
                        animator.SetInteger(content.animName, int.Parse(content.value));
                    else
                        animator.SetInteger(content.animName, 0);
                    break;
                case Content.ParamType.Bool:
                    if (Input.GetKey(content.keyCode))
                        animator.SetBool(content.animName, bool.Parse(content.value));
                    else
                        animator.SetBool(content.animName, !bool.Parse(content.value));
                    break;
                case Content.ParamType.Trigger:
                    if (Input.GetKeyDown(content.keyCode))
                        animator.SetTrigger(content.animName);
                    break;
            }
        }

        animator.SetBool("isRun", true);
        animator.SetFloat("DirectionalBlend", brend);
    }
}
