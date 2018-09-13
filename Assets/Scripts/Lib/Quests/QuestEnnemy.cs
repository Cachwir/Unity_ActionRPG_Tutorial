using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class QuestEnnemy : MonoBehaviour {

    public Quest quest;
    public string onEnnemyKillFunction;

    protected EnnemyController ennemyController;

	// Use this for initialization
	void Start () {
        ennemyController = GetComponent<EnnemyController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    protected void OnEnnemyKill()
    {
        if (onEnnemyKillFunction != "")
        {
            ClassReflector.DynamicInvoke(quest, onEnnemyKillFunction, ennemyController);
        }
    }
}
