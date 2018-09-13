using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEndingDialogHolder : QuestStepDialogHolder
{
    // Use this for initialization
    protected new void Start()
    {
        base.Start();
        onDialogCloseCallback = delegate()
        {
            if (IsQuestValid())
            {
                quest.EndQuestWithSuccess();
            }
            else
            {
                quest.EndQuestWithFailure();
            }
        };
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
    }

    protected bool IsQuestValid()
    {
        return true;
    }
}
