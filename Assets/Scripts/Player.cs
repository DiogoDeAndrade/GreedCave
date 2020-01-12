using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public ControllerMapping    controlMap;
    public float                moveSpeed = 1.5f;
    public float                velocityY;

    CharacterController charController;
    Vector3             deltaPos;

    protected override void Start()
    {
        base.Start();

        charController = GetComponent<CharacterController>();
    }

    protected void FixedUpdate()
    {
        // Apply gravity
        if (!isGrounded)
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

    protected override void Update()
    {
        desiredDir = Vector2.zero;
        desiredDir.x = Input.GetAxis(controlMap.horizontalAxis);
        desiredDir.y = Input.GetAxis(controlMap.verticalAxis);

        if (Input.GetButtonDown(controlMap.attackButton))
        {
            RunAttack();
        }

        base.Update();

        deltaPos = new Vector3(desiredDir.x * moveSpeed, 0.0f, desiredDir.y * moveSpeed);
    }

    protected override void DoKnockback(float time, float strength, Vector3 dir)
    {
        base.DoKnockback(time, strength, dir);

        velocityY = 1;
    }
}
