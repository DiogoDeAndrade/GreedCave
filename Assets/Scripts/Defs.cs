using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { North, East, South, West };
public enum StatType { Constitution = 0, Strength = 1, Intelect = 2, Dexterity = 3, Spirit = 4, 
                       MaxHP, MaxResource, AttackSpeedModifier, PhysicalDamage, MagicalDamage, RangedDamage, CritRate, ResourceRegen, Dodge };

static public class Consts
{
    public const int Stats = 5;
};

static public class Helper
{
    public static Direction GetDirectionFromVector(Vector2 dir)
    {
        dir.y *= 0.5f;
        dir.Normalize();

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0.0f) return Direction.East;
            else return Direction.West;
        }
        else
        {
            if (dir.y > 0.0f) return Direction.North;
            else return Direction.South;
        }
    }

    public static Vector2 GetVectorFromDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return new Vector2(0.0f, 1.0f);
            case Direction.East:
                return new Vector2(1.0f, 0.0f);
            case Direction.South:
                return new Vector2(0.0f, -1.0f);
            case Direction.West:
                return new Vector2(-1.0f, 0.0f);
            default:
                return new Vector2(0.0f, 0.0f);
        }
    }
}
