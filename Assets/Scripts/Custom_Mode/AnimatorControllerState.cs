using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControllerState : MonoBehaviour
{
    public Animator animator;
    public int AnimationHash;
    public int isSittingHash;
    public int isRunningHash;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        AnimationHash = Animator.StringToHash("animation");

    }
}
