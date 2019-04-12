using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Prop
{

    public Vector2 direction;
    public Animator animator;

    public void PlayTriggerAnimation(string _animation) {
        animator.Play(_animation);
    } 
}
