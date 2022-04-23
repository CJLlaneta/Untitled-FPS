using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private static AnimationManager _instance;

    public static AnimationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("AnimationManager");
                go.AddComponent<AnimationManager>();
                _instance = go.GetComponent<AnimationManager>();

            }
            return _instance;
        }

    }
    public void ActivateAnimation(Animator Animator)
    {
        Animator.enabled = true;
    }
    public void StopAnimation(Animator Animator)
    {
        Animator.enabled = false;
    }
    public void SetAnimationBoolean(Animator Animator, string VariableName, bool Status)
    {
        if (!Animator.enabled)
        {
            Animator.enabled = true;
        }
        Animator.SetBool(VariableName, Status);
    }
    public bool IsAnimationClipPlaying(Animator Animator, string AnimationClip)
    {
        bool _ret = false;
        _ret = Animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationClip);
        return _ret;
    }
    public void PlayClip(Animator Animator, string AnimationClip)
    {
        Animator.Play(AnimationClip, 0, 0f);
    }

    public void StopClip(Animator Animator, string AnimationClip)
    {
        Animator.Play(AnimationClip, 0, 0f);
    }
    public void SetAnimationTrigger()
    {

    }

}
