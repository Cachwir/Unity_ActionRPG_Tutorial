using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Cinematic_PostCinematicCallback();

public class Cinematic : MonoBehaviour
{
    public bool deactivateCameraAutoControl;
    public Cinematic_PostCinematicCallback postCinematicCallback = delegate () {};

    protected List<CinematicStep> cinematicSteps = new List<CinematicStep>();
    protected CameraController originalCameraController;
    protected CinematicsManager cinematicsManager;
    public bool HasCinematicStarted { get; set; }
    public bool HasCinematicEnded { get; set; }
    protected int currentCinematicStep;

	// Use this for initialization
	void Start () {
        originalCameraController = FindObjectOfType<CameraController>();
        cinematicsManager = FindObjectOfType<CinematicsManager>();

        foreach (Transform child in transform)
        {
            CinematicStep cinematicStep = child.GetComponent<CinematicStep>();

            if (cinematicStep != null)
            {
                cinematicSteps.Add(cinematicStep);
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (HasCinematicEnded)
        {
            if (cinematicSteps[currentCinematicStep].HasStepEnded)
            {
                if (currentCinematicStep < (cinematicSteps.Count - 1))
                {
                    currentCinematicStep++;
                    StartCinematicStep(currentCinematicStep);
                }
                else
                {
                    EndCinematic();
                }
            }
        }
	}

    // originalCameraController.SwitchToManualControl(); or originalCameraController.setTarget(newTarget);
    // ... do some stuff ...
    // originalCameraController.SwitchToAutomaticControl();
    // cinematicsManager.StopCinematic(this);
    public void StartCinematic()
    {
        if (cinematicSteps.Count == 0)
        {
            throw new System.Exception("The cinematic should have at least one cinematic scene");
        }

        HasCinematicEnded = true;
        currentCinematicStep = 0;

        if (deactivateCameraAutoControl)
        {
            originalCameraController.SwitchToManualControl();
        }

        StartCinematicStep(currentCinematicStep);
    }

    public void StartCinematicStep(int cinematicStepId)
    {
        cinematicSteps[cinematicStepId].StartStep();
    }

    public void EndCinematic()
    {
        postCinematicCallback();
        originalCameraController.SwitchToAutomaticControl();
    }
    
    public void ForceEndCinematic()
    {
        foreach(CinematicStep cinematicStep in cinematicSteps)
        {
            cinematicStep.ForceEndStep();
        }
    }
}
