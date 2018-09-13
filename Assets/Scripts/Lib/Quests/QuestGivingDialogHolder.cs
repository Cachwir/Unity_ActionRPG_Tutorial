using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGivingDialogHolder : QuestStepDialogHolder
{
    // Use this for initialization
    protected new void Start () {
        base.Start();
        quest = questReference.Quest;
        onDialogCloseCallback = delegate ()
        {
            quest.StartQuest();
            ExecuteOnDialogClose();
        };
    }
	
	// Update is called once per frame
	protected new void Update () {
        base.Update();
    }

    protected override bool CanStartDialog()
    {
        return quest.CanStartQuest() && !hasRead;
    }
}
