using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public ControllerMapping    controlMap;
    public float                moveSpeed = 1.5f;

    CharacterController charController;

    protected override void Start()
    {
        base.Start();

        charController = GetComponent<CharacterController>();
    }

    protected override void Update()
    {
        desiredDir = Vector2.zero;
        desiredDir.x = Input.GetAxis(controlMap.horizontalAxis);
        desiredDir.y = Input.GetAxis(controlMap.verticalAxis);

        charController.Move(new Vector3(desiredDir.x * moveSpeed * Time.deltaTime, 0.0f, desiredDir.y * moveSpeed * Time.deltaTime));

        base.Update();
    }
}
