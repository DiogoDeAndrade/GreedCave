using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "GreedCave/Character Class")]
public class CharacterClass : ScriptableObject
{
    public RuntimeAnimatorController    controller;
    public Weapon                       weapon;
    public bool                         allowKnockback = true;
    [Header("Stats")]
    public Stats                        baseStats;
    public Stats                        autoLevelUp;
}
