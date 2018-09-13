using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

    public DialogManager dialogManager;

    protected List<Quest> quests = new List<Quest>();

	// Use this for initialization
	void Start ()
    {
        foreach (Transform child in transform)
        {
            Quest quest = child.GetComponent<Quest>();

            if (quest != null)
            {
                quests.Add(quest);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<Quest> GetQuests()
    {
        return quests;
    }

    public void DisplayQuestDescription(Quest quest)
    {
        dialogManager.OpenDialog(quest._description);
    }

    public Quest GetQuestBySlug(string slug)
    {
        foreach (Quest quest in quests)
        {
            if (quest._slug == slug)
            {
                return quest;
            }
        }

        throw new System.Exception("No quest found with the slug : " + slug);
    }
}
