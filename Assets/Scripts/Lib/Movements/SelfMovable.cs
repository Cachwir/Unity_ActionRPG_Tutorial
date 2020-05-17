using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfMovable : Movable {

    protected bool isMovingSelf;
    protected Vector2 selfMove;
    protected float movementDuration = 0;
    protected MoveCompiler.Movable_OnMoveFactorEndCallback afterMoveCallback;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        afterMoveCallback = delegate () { };
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    protected void FixedUpdate()
    {
        MoveSelf();
    }

    protected void MoveSelf()
    {
        if (isMovingSelf && selfMove != Vector2.zero)
        {
            MoveCompiler.AddOrEditSelfMoveFactor(selfMove, movementDuration > 0, movementDuration, afterMoveCallback);
        }
    }

    public override bool ValidateMoveDirection(Vector3 velocity)
    {
        return base.ValidateMoveDirection(velocity);
    }
}
