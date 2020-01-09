using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GreedCave/Controller Mapping")]
public class ControllerMapping : ScriptableObject
{
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public string attackButton = "Fire1";
    public string secondaryAttackButton = "Fire2";
    public string specialButton = "Fire3";
}
