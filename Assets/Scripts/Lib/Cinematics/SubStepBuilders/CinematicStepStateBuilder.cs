using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicStepStateBuilder : CinematicSubStep
{
    public StepStateHolder stepStateHolder;

    public override void StartSubStep()
    {
        RegisterStepState();
    }

    public void RegisterStepState()
    {
        stepStateHolder.Register();
        HasSubStepEnded = true;
    }
}
