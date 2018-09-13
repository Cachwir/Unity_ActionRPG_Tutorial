using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicsManager : MonoBehaviour, IGameLoadListener
{
    protected CameraController originalCameraController;
    protected PlayerController playerController;

	// Use this for initialization
	void Start () {
        originalCameraController = FindObjectOfType<CameraController>();
        playerController = FindObjectOfType<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartCinematic(Cinematic cinematic)
    {
        EnterCinematicState();

        cinematic.postCinematicCallback = delegate ()
        {
            StopCinematic(cinematic);
        };
        cinematic.StartCinematic();
    }

    public void StopCinematic(Cinematic cinematic)
    {
        LeaveCinematicState();
    }

    public void EnterCinematicState()
    {
        PausePlayerActions();
    }

    public void LeaveCinematicState()
    {
        ResumePlayerActions();
        // TODO : Set the originalCamera as Main
        // TODO : Makes the fact that the cinematic has been seen as savable
    }

    public void PausePlayerActions()
    {
        playerController.RestrainPlayer();
    }

    public void ResumePlayerActions()
    {
        playerController.UnrestrainPlayer();
    }

    public void ForceEndCinematics()
    {
        foreach (Cinematic cinematic in FindObjectsOfType<Cinematic>())
        {
            cinematic.ForceEndCinematic();
        }
    }

    public void OnGameLoad()
    {
        ForceEndCinematics(); // ends all cinematics
    }
}
