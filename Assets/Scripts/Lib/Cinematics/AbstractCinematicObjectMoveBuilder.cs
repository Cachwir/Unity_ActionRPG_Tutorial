using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractCinematicObjectMoveBuilder : CinematicSubStep
{
    public GameObject targetDestination;
    public float moveSpeed = 2.0f;
    public float delay;
    public CinematicObjectMove.MoveType moveType = CinematicObjectMove.MoveType.LINEAR;
    public CinematicSubStep_Callback afterMoveCallback = delegate () { };

    protected GameObject target;
    protected bool _isZPositionFixed = true; // If true, the z position of this gameObject will be fixed (in order to avoid some bugs)
    protected float _zPositionFixedValue = 0; // The value the z position will be fixed to
    protected CinematicObjectMove cinematicObjectMove;
    protected bool hasStartedMoving;

    protected void Update()
    {
        CheckAllMovesCompleted();
    }

    public void CheckAllMovesCompleted()
    {
        if (hasStartedMoving && cinematicObjectMove != null && cinematicObjectMove.HasFinishedMoving)
        {
            Destroy(cinematicObjectMove);

            HasSubStepEnded = true;
            afterMoveCallback();
        }
    }

    public void BuildCinematicObjectMove()
    {
        cinematicObjectMove = target.AddComponent<CinematicObjectMove>();
        cinematicObjectMove.targetDestination = targetDestination;
        cinematicObjectMove.moveSpeed = moveSpeed;
        cinematicObjectMove.isZPositionFixed = _isZPositionFixed;
        cinematicObjectMove.zPositionFixedValue = _zPositionFixedValue;
        cinematicObjectMove.moveType = moveType;
    }

    public override void StartSubStep()
    {
        StartCoroutine("StartAfterDelay");
    }

    public override void ForceEndSubStep()
    {
        StopCoroutine("StartAfterDelay");

        CinematicObjectMove cinematicObjectMove = target.GetComponent<CinematicObjectMove>();
        if (cinematicObjectMove)
        {
            Destroy(cinematicObjectMove);
        }
    }

    public void StartMoving()
    {
        BuildCinematicObjectMove();
        cinematicObjectMove.StartMoving();
        hasStartedMoving = true;
    }

    IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        StartMoving();
    }
}
