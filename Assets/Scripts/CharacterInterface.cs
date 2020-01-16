using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInterface : MonoBehaviour
{
    public float        moveSpeed = 1.5f;

    [HideInInspector] 
    public Character    character;
    protected float     knockbackTime = 0;
    protected float     knockbackStrength = 0;
    protected Vector3   knockbackDir;
    protected float     velocityY;
    protected Vector3   deltaPos;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    protected void FixedUpdate()
    {
        // Apply gravity
        if (!character.isGrounded)
        {
            velocityY += Physics.gravity.y * Time.fixedDeltaTime;

        }
        else
        {
            if (velocityY < 0.0f) velocityY = 0;
        }

        Vector3 toMove = deltaPos;

        toMove.y = velocityY;

        if (knockbackTime > 0.0f)
        {
            knockbackTime -= Time.deltaTime;

            toMove += knockbackDir.x0z() * knockbackStrength;
        }

        ActualMove(toMove);
    }

    protected virtual void ActualMove(Vector3 toMove)
    {

    }

    public virtual void DoKnockback(float time, float strength, Vector3 dir)
    {
        knockbackTime = time;
        knockbackStrength = strength;
        knockbackDir = dir;
    }

    public virtual void OnDeath()
    {
    }
}
