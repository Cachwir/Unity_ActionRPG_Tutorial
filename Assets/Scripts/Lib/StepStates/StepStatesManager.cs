using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class StepStatesManager : MonoBehaviour, IGameLoadListener
{
    public Dictionary<long, string> CompletedStepStates = new Dictionary<long, string>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RegisterStepState(StepStateHolder stepStateHolder)
    {
        CompletedStepStates.Add(Helper.GetObjectLocalIdInFile(stepStateHolder), stepStateHolder.StepStateSlug);
    }

    public void LoadStepState(StepStateHolder stepStateHolder)
    {
        if (CompletedStepStates.ContainsKey(Helper.GetObjectLocalIdInFile(stepStateHolder)))
        {
            string methodName = "LoadStepState" + Helper.UcFirst(stepStateHolder.StepStateSlug);

            if (ClassReflector.HasMethod(this, methodName))
            {
                ClassReflector.DynamicInvoke(this, methodName, stepStateHolder);
            }
            else
            {
                throw new System.Exception("The method " + methodName + " doesn't exist in the current class.");
            }
        }
    }

    public void LoadStepStateActivate(StepStateHolder stepStateHolder)
    {
        stepStateHolder.affectedGameObject.SetActive(false);
    }

    public void LoadStepStateDeactivate(StepStateHolder stepStateHolder)
    {
        stepStateHolder.affectedGameObject.SetActive(false);
    }

    public void ReloadAllStepStatesHolderState()
    {
        foreach (StepStateHolder stepStateHolder in FindObjectsOfType<StepStateHolder>())
        {
            stepStateHolder.LoadStepState();
        }
    }

    public void EmptyStepStates()
    {
        CompletedStepStates.Clear();
    }

    public void OnGameLoad()
    {
        EmptyStepStates(); // empties all stored step states
    }
}
