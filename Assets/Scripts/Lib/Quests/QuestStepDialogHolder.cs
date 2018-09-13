using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

public class QuestStepDialogHolder : DialogHolder
{
    public QuestReference questReference; // The reference to the Quest
    public string reachConditionName; // (optionnal) A bool property of method name inside the quest object to check if this player can reach this step
    public string onDialogCloseFunction; // (optionnal) A method to call in the quest object once the dialog had ended
    public StepStateHolder stepStateToRegisterOnDialogClose; // (optionnal) Saves target QuestStepStateHolder so when the user loads the scene again, it shall remember its target gameObject's state

    protected Quest quest; // The associated quest object
    protected bool hasRead;

    // Use this for initialization
    protected new void Start()
    {
        base.Start();
        quest = questReference.Quest;
        onDialogCloseCallback = delegate ()
        {
            ExecuteOnDialogClose();
        };
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
    }

    protected override bool CanStartDialog()
    {
        return quest.IsActive() && IsStepReachable() && !hasRead;
    }

    public bool IsStepReachable()
    {
        if (reachConditionName == "")
        {
            return true;
        }
        else if (ClassReflector.HasProperty(quest, reachConditionName))
        {
            return ClassReflector.GetBoolProperty(quest, reachConditionName);
        }
        else if (ClassReflector.HasMethod(quest, reachConditionName))
        {
            return ClassReflector.DynamicInvokeBool(quest, reachConditionName, this);
        }
        else
        {
            throw new System.Exception("The given reachConditionName (" + reachConditionName + ") isn't an existing property or method in the associated quest." );
        }
    }

    public void ExecuteOnDialogClose()
    {
        if (onDialogCloseFunction != "" && ClassReflector.HasMethod(quest, onDialogCloseFunction))
        {
            ClassReflector.DynamicInvoke(quest, onDialogCloseFunction, this);
        }
        if (stepStateToRegisterOnDialogClose != null)
        {
            stepStateToRegisterOnDialogClose.Register();
        }

        hasRead = true;
    }
}
