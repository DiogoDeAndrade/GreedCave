using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEventsProcessor : MonoBehaviour
{
    Character character;

    void Start()
    {
        character = GetComponentInParent<Character>();
    }
    
    public void Melee()
    {
        character.DoMeleeDamage();
    }
}
