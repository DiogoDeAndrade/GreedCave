using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterInterface : CharacterInterface
{
    [Header("Player")]
    public ControllerMapping    controlMap;
    public float                velocityY;

    CharacterController charController;
    Vector3             deltaPos;

    protected void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    protected void FixedUpdate()
    {
        // Apply gravity
        if (!character.isGrounded)
        {
            velocityY += Physics.gravity.y * Time.fixedDeltaTime;

        }
        else
        {
            if (velocityY < 0.0f) velocityY = 0;
        }

        Vector3 toMove = deltaPos;

        toMove.y = velocityY;

        if (knockbackTime > 0.0f)
        {
            knockbackTime -= Time.deltaTime;

            toMove += knockbackDir.x0z() * knockbackStrength;
        }

        charController.Move(toMove * Time.fixedDeltaTime);
    }

    protected void Update()
    {
        if (character.isDead)
        {
            if (character.timeSinceDeath > 5.0f)
            {
                // Check if all players are dead
                if (Character.AllAreDead())
                {
                    // Game over
                }
                else
                {
                    // Respawn
                    character.Respawn();
                }
            }
        }

        Vector2 desiredDir = Vector2.zero;
        desiredDir.x = Input.GetAxis(controlMap.horizontalAxis);
        desiredDir.y = Input.GetAxis(controlMap.verticalAxis);

        character.SetDesiredDir(desiredDir);

        if (Input.GetButtonDown(controlMap.attackButton))
        {
            character.RunAttack();
        }

        deltaPos = new Vector3(desiredDir.x * moveSpeed, 0.0f, desiredDir.y * moveSpeed);
    }

    public override void DoKnockback(float time, float strength, Vector3 dir)
    {
        base.DoKnockback(time, strength, dir);

        velocityY = 1;
    }
}
