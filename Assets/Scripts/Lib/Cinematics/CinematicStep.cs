using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicStep : MonoBehaviour {

    protected List<CinematicSubStep> subSteps = new List<CinematicSubStep>();

    public bool HasStepStarted { get; set; }
    public bool HasStepEnded { get; set; }

    // Use this for initialization
    void Start ()
    {
        foreach (Transform child in transform)
        {
            CinematicSubStep cinematicSubStep = child.GetComponent<CinematicSubStep>();

            if (cinematicSubStep != null)
            {
                subSteps.Add(cinematicSubStep);
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        CheckIfHaveSubStepsEndedOnUpdate();
    }

    public void CheckIfHaveSubStepsEndedOnUpdate()
    {
        if (HasStepStarted && !HasStepEnded)
        {
            bool haveAllEnded = true;

            foreach (CinematicSubStep subStep in subSteps)
            {
                if (!subStep.HasSubStepEnded)
                {
                    haveAllEnded = false;
                }
            }

            if (haveAllEnded)
            {
                HasStepEnded = true;
            }
        }
    }

    public void StartStep()
    {
        foreach (CinematicSubStep subStep in subSteps)
        {
            subStep.StartSubStep();
        }

        HasStepStarted = true;
    }

    public void ForceEndStep()
    {
        foreach (CinematicSubStep subStep in subSteps)
        {
            subStep.ForceEndSubStep();
        }
    }
}
