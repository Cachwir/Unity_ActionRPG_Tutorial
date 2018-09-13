using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class EnnemyController : AutoMoving, IPointerClickHandler
{
    protected SFXController sfxController;

    // Use this for initialization
    new void Start ()
    {
        base.Start();
        
        sfxController = FindObjectOfType<SFXController>();
    }
	
	// Update is called once per frame
	new void Update ()
    {
        base.Update();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        sfxController.PlaySoundEffect("cuteCreatureHit");
    }
}
