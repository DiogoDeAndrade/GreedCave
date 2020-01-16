using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Character : MonoBehaviour
{
    public CharacterClass   characterClass;
    public GameObject       gfx;
    public int              level = 1;

    [Header("Visuals")]
    public bool             combatDamageEnabled = true;
    public Transform        combatTextSpawnPoint;

    protected Animator          animator;
    protected SpriteRenderer    spriteRenderer;
    protected Vector2           desiredDir;
    protected float             knockbackTime = 0;
    protected float             knockbackStrength = 0;
    protected Vector3           knockbackDir;
    protected Vector2           lastMoveDir = new Vector2(0.0f, -1.0f);
    protected float             attackCooldown;
    protected Stats             currentStats;
    protected float             animSpeed = 1.0f;

    [ShowNonSerializedField]
    protected float             currentHP;

    Coroutine   flashCR;
    Material    spriteMaterial;
    Stats       baseStats;
    bool        damageAlreadyApplied = false;

    protected bool isGrounded
    {
        get
        {
            return (transform.position.y < 0.1f);
        }
    }

    protected bool isDead
    {
        get
        {
            return currentHP <= 0;
        }
    }

    protected bool isInvulnerable
    {
        get
        {
            return isDead;
        }
    }

    protected virtual void Start()
    {
        animator = gfx.GetComponent<Animator>();
        spriteRenderer = gfx.GetComponent<SpriteRenderer>();
        spriteMaterial = spriteRenderer.material;

        animator.runtimeAnimatorController = characterClass.controller;

        baseStats = characterClass.baseStats.Clone();

        UpdateStats();

        currentHP = currentStats.Get(StatType.MaxHP);
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
        animator.SetFloat("AnimSpeed", animSpeed);

        UpdateStats();

        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0)
            {
                animSpeed = 1.0f;
                attackCooldown = 0;
            }
        }
    }

    void UpdateStats()
    {
        currentStats = baseStats.Clone();
    }

    protected bool RunAttack()
    {
        if (characterClass.weapon == null) return false;
        if (attackCooldown > 0) return false;

        switch (characterClass.weapon.animationType)
        {
            case Weapon.AnimType.Slash:
                animator.SetTrigger("Slash");
                break;
            case Weapon.AnimType.Shoot:
                animator.SetTrigger("Shoot");
                break;
            case Weapon.AnimType.Thrust:
                animator.SetTrigger("Thrust");
                break;
            case Weapon.AnimType.Cast:
                animator.SetTrigger("Cast");
                break;
            default:
                break;
        }

        float attackSpeedModifier = currentStats.Get(StatType.AttackSpeedModifier);
        attackCooldown = characterClass.weapon.attackCooldown * attackSpeedModifier;
        animSpeed = 1.0f / attackSpeedModifier;

        damageAlreadyApplied = false;

        return true;
    }

    public void DoMeleeDamage()
    {
        if (damageAlreadyApplied) return;

        damageAlreadyApplied = true;

        // Check where player is facing
        var dir = Helper.GetDirectionFromVector(lastMoveDir);

        var vec = Helper.GetVectorFromDirection(dir);

        Vector3 damagePos = new Vector3(vec.x, 0.0f, vec.y);

        damagePos = transform.position + characterClass.weapon.attackRadius * damagePos + Vector3.up;

        // If a character has multiple colliders, it can be processed multiple times
        List<Character> alreadyProcessed = new List<Character>();

        // The event can also be called multiple times because of the blend trees, need to check if this damage has already been applied
        Collider[] colliders = Physics.OverlapSphere(damagePos, characterClass.weapon.attackRadius, LayerMask.GetMask("Player", "Enemy"));
        foreach (var collider in colliders)
        {
            Character character = collider.GetComponent<Character>();
            if ((character != null) && (character != this) && (!alreadyProcessed.Contains(character)) && (!character.isInvulnerable))
            {
                alreadyProcessed.Add(character);

                character.DealDamage(characterClass.weapon, characterClass.weapon.ComputeDamage(level), damagePos);
            }
        }
    }

    public void DealDamage(Weapon weapon, float damage, Vector3 damageSourcePos)
    {
        if (combatDamageEnabled)
        {
            CombatTextManager.SpawnText(combatTextSpawnPoint.gameObject, damage, "-{0}", new Color(0.7f, 0.2f, 0.2f, 1), new Color(0.7f, 0.2f, 0.2f, 0));
        }

        currentHP -= damage;

        if (currentHP <= 0)
        {
            animator.SetTrigger("Death");
        }
        else
        {
            Flash(Color.red);

            if ((weapon.knockback) && (characterClass.allowKnockback))
            {
                DoKnockback(weapon.knockbackTime, weapon.knockbackStrength, transform.position - damageSourcePos);
            }
        }
    }

    protected virtual void DoKnockback(float time, float strength, Vector3 dir)
    {
        knockbackTime = time;
        knockbackStrength = strength;
        knockbackDir = dir;
    }

    public void Flash(Color c)
    {
        if (flashCR != null)
        {
            StopCoroutine(flashCR);
        }

        flashCR = StartCoroutine(FlashCR(c));
    }

    IEnumerator FlashCR(Color startColor)
    {
        Color c = startColor;

        while (c.a > 0.0f)
        {
            spriteMaterial.SetColor("_Color", c);

            c.a -= Time.deltaTime * 2.0f;

            yield return null;
        }

        c.a = 0;
        spriteMaterial.SetColor("_Color", c);

        flashCR = null;
    }

    private void OnDrawGizmos()
    {
        // Display damage areas for melee
        /*if (characterClass.weapon)
        {
            for (int i = 0; i < 4; i++)
            {
                var vec = Helper.GetVectorFromDirection((Direction)i);

                Vector3 damagePos = new Vector3(vec.x, 0.0f, vec.y);

                damagePos = transform.position + characterClass.weapon.attackRadius * damagePos + Vector3.up * 0.5f;

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(damagePos, characterClass.weapon.attackRadius);
            }
        }//*/
    }
}
