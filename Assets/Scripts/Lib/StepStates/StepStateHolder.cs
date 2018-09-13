using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepStateHolder : MonoBehaviour {

    public enum BasicStepState
    {
        none, // uses orStepStateSlug's value
        activate, // activates the gameObject
        deactivate // deactivates the gameObject
    };
    
    public GameObject affectedGameObject;
    public BasicStepState stepState; // if none is chosen, orStepStateSlug will be used instead
    public string orStepStateSlug;

    protected StepStatesManager stepStateManager;
    public string StepStateSlug { get; set; }

    // Use this for initialization
    void Start () {
        stepStateManager = FindObjectOfType<StepStatesManager>();
        StepStateSlug = stepState == BasicStepState.none ? orStepStateSlug : stepState.ToString();
        LoadStepState();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void LoadStepState()
    {
        stepStateManager.LoadStepState(this);
    }

    // Called when the step has been validated
    public void Register()
    {
        stepStateManager.RegisterStepState(this);
    }
}
