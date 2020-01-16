using NaughtyAttributes;
using System;

[Serializable]
public class Stats 
{
    public float constitution;
    public float strength;
    public float intelect;
    public float dexterity;
    public float spirit;

    public Stats()
    {
        constitution = strength = intelect = dexterity = spirit = 0.0f;
    }

    public Stats(Stats src)
    {
        constitution = src.constitution;
        strength = src.strength;
        intelect = src.intelect;
        dexterity = src.dexterity;
        spirit = src.spirit;
    }

    public float Get(StatType type)
    {
        switch (type)
        {
            case StatType.Constitution:
                return constitution;
            case StatType.Strength:
                return strength;
            case StatType.Intelect:
                return intelect;
            case StatType.Dexterity:
                return dexterity;
            case StatType.Spirit:
                return spirit;
            case StatType.MaxHP:
                return (constitution + strength) * 10;
            case StatType.MaxResource:
                return intelect * 10;
            case StatType.AttackSpeedModifier:
                return 100.0f / (100.0f + dexterity * 2.0f);
            case StatType.PhysicalDamage:
                return strength;
            case StatType.MagicalDamage:
                return intelect;
            case StatType.RangedDamage:
                return dexterity;
            case StatType.CritRate:
                return (intelect + dexterity) / 100.0f;
            case StatType.ResourceRegen:
                return 0.1f + spirit / 100.0f;
            case StatType.Dodge:
                return 0.1f + dexterity / 100.0f;
            default:
                break;
        }

        return 0;
    }

    public void Set(StatType type, float val)
    {
        switch (type)
        {
            case StatType.Constitution:
                constitution = val;
                break;
            case StatType.Strength:
                strength = val;
                break;
            case StatType.Intelect:
                intelect = val;
                break;
            case StatType.Dexterity:
                dexterity = val;
                break;
            case StatType.Spirit:
                spirit = val;
                break;
            default:
                break;
        }
    }

    public Stats Clone()
    {
        return new Stats(this);                
    }
}
