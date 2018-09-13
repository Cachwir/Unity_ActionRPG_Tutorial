using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReference : MonoBehaviour {

    public string questSlug;

    public Quest Quest { get; set; }

    // Use this for initialization
    void Start () {
        Quest = FindObjectOfType<QuestManager>().GetQuestBySlug(questSlug);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
