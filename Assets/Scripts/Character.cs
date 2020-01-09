using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterClass   characterClass;
    public GameObject       gfx;

    protected Animator      animator;
    protected Vector2       desiredDir;

    Vector2 lastMoveDir = new Vector2(0.0f, -1.0f);

    protected virtual void Start()
    {
        animator = gfx.GetComponent<Animator>();

        animator.runtimeAnimatorController = characterClass.controller;
    }

    protected virtual void Update()
    {
        if (desiredDir.magnitude > 0.1f)
        {
            lastMoveDir = desiredDir.normalized;
        }

        animator.SetFloat("DirX", lastMoveDir.x);
        animator.SetFloat("DirY", lastMoveDir.y);

        animator.SetFloat("Speed", desiredDir.magnitude);
    }
}
