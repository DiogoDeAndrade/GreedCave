using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GreedCave/Weapon")]
public class Weapon : ScriptableObject
{
    public enum Type { Melee, Ranged, Magic };
    public enum AnimType { Slash, Shoot, Thrust, Cast };

    public Type     type;
    public float    baseDamage;
    public float    damagePerLevel;

    [Header("Collision detection and response")]
    public float    attackRadius = 1.0f;
    public bool     knockback
    {
        get { return type == Type.Melee; }
    }
    public float    knockbackTime = 0.2f;
    public float    knockbackStrength = 0.1f;

    [Header("Visuals")]
    public AnimType animationType;


    public float ComputeDamage(int level)
    {
        return baseDamage + damagePerLevel * level;
    }
}
