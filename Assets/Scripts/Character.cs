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
    public ParticleSystem   deathPS;

    protected Animator          animator;
    protected SpriteRenderer    spriteRenderer;
    protected Vector2           desiredDir;
    protected Vector2           lastMoveDir = new Vector2(0.0f, -1.0f);
    protected float             attackCooldown;
    protected Stats             currentStats;
    protected float             animSpeed = 1.0f;
    [HideInInspector]
    public float                timeSinceDeath;
    [ShowNonSerializedField]
    protected float             currentHP;

    Coroutine           flashCR;
    Material            spriteMaterial;
    Stats               baseStats;
    bool                damageAlreadyApplied = false;
    CharacterInterface  charInterface;
    float               invulnerabilityTimer = 0.0f;
    float               invulnerabilitySubTimer = 0.0f;

    public bool isGrounded
    {
        get
        {
            return (transform.position.y < 0.1f);
        }
    }

    public bool isDead
    {
        get
        {
            return currentHP <= 0;
        }
    }

    public bool isInvulnerable
    {
        get
        {
            if (invulnerabilityTimer > 0) return true;

            return isDead;
        }
        set
        {
            if (value)
            {
                invulnerabilityTimer = 2.0f;
                invulnerabilitySubTimer = 0.0f;
            }
            else
            {
                invulnerabilityTimer = 0.0f;
            }
        }
    }

    protected virtual void Start()
    {
        animator = gfx.GetComponent<Animator>();
        spriteRenderer = gfx.GetComponent<SpriteRenderer>();
        charInterface = GetComponent<CharacterInterface>();
        spriteMaterial = spriteRenderer.material;

        animator.runtimeAnimatorController = characterClass.controller;

        baseStats = characterClass.baseStats.Clone();

        UpdateStats();

        currentHP = currentStats.Get(StatType.MaxHP);

        StartCoroutine(LateStartCR());
    }

    IEnumerator LateStartCR()
    {
        yield return null;

        InitFX();
    }

    void InitFX()
    {
        var shape = deathPS.shape;

        shape.spriteRenderer = spriteRenderer;
        shape.texture = spriteRenderer.sprite.texture;
    }

    public void SetDesiredDir(Vector2 dir)
    {
        desiredDir = dir;
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

        if (isDead)
        {
            timeSinceDeath += Time.deltaTime;
        }

        if (invulnerabilityTimer > 0)
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer <= 0)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                invulnerabilitySubTimer += Time.deltaTime;
                if (invulnerabilitySubTimer > 0.1f)
                {
                    spriteRenderer.enabled = !spriteRenderer.enabled;
                    invulnerabilitySubTimer = 0.0f;
                }
            }
        }
    }

    void UpdateStats()
    {
        currentStats = baseStats.Clone();
    }

    public bool RunAttack()
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
            spriteRenderer.enabled = false;
            deathPS.Play();
            timeSinceDeath = 0.0f;

            OnDeath();
        }
        else
        {
            Flash(Color.red);

            if ((weapon.knockback) && (characterClass.allowKnockback))
            {
                charInterface.DoKnockback(weapon.knockbackTime, weapon.knockbackStrength, transform.position - damageSourcePos);
            }
        }
    }

    public void Respawn()
    {
        animator.SetTrigger("Reset");
        currentHP = currentStats.Get(StatType.MaxHP);
        spriteRenderer.enabled = true;
        timeSinceDeath = 0.0f;
        isInvulnerable = true;
    }

    protected virtual void OnDeath()
    {

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

    public void EnableColliders(bool b)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = b;
        }
    }

    static public bool AllAreDead()
    {
        PlayerCharacterInterface[] players = GameObject.FindObjectsOfType<PlayerCharacterInterface>();
        
        foreach (var player in players)
        {
            if (!player.character.isDead)
            {
                return false;
            }
        }

        return true;
    }
}
