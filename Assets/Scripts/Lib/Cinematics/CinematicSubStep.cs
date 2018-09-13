using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CinematicSubStep_Callback();

public abstract class CinematicSubStep : MonoBehaviour
{
    public bool HasSubStepEnded { get; set; }

    public virtual void StartSubStep()
    {

    }

    public virtual void ForceEndSubStep()
    {
        
    }
}
