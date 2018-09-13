using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicDialogBuilder : CinematicSubStep
{
    public DialogHolder dialogHolder;
    public float delay;

    public override void StartSubStep()
    {
        StartCoroutine("StartAfterDelay");
    }

    public override void ForceEndSubStep()
    {
        StopCoroutine("StartAfterDelay");
    }

    public void StartDialog()
    {
        dialogHolder.onDialogCloseCallback = delegate ()
        {
            HasSubStepEnded = true;
        };
        dialogHolder.StartOwnDialog(true);
    }

    IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        StartDialog();
    }
}
