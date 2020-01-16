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

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public virtual void DoKnockback(float time, float strength, Vector3 dir)
    {
        knockbackTime = time;
        knockbackStrength = strength;
        knockbackDir = dir;
    }

    protected virtual void OnDeath()
    {
    }
}
