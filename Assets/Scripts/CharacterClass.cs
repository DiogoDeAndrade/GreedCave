using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GreedCave/Character Class")]
public class CharacterClass : ScriptableObject
{
    public RuntimeAnimatorController    controller;
    public Weapon                       weapon;
    public bool                         allowKnockback = true;
}
