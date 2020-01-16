using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterInterface : CharacterInterface
{
    [Header("Player")]
    public ControllerMapping    controlMap;

    CharacterController charController;

    protected void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    protected override void ActualMove(Vector3 toMove)
    {
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
