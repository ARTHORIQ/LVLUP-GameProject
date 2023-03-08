using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChecker : MonoBehaviour
{
    public AnimationClip clipToCheck;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(clipToCheck.name))
        {
            Debug.Log("The animation clip " + clipToCheck.name + " is currently playing.");
        }
    }
}