using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHolder : MonoBehaviour {
    
    public string note;

    public List<string> dialogue;

    protected DialogManager dialogManager;
    protected PlayerController playerController;
    protected KeyEventController keyEventController;

    public onDialogClose onDialogCloseCallback;

    // Use this for initialization
    protected void Start () {
        dialogManager = FindObjectOfType<DialogManager>();
        playerController = FindObjectOfType<PlayerController>();
        keyEventController = FindObjectOfType<KeyEventController>();
    }

    // Update is called once per frame
    protected void Update ()
    {

    }

    protected void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Player"
            && keyEventController.IsKeyPressed(KeyCode.Space)
            && !DialogManager.isReading 
            && CanStartDialog())
        {
            keyEventController.AllocateKeyPress(KeyCode.Space);
            StartOwnDialog();
        }
    }

    protected virtual bool CanStartDialog()
    {
        return true;
    }

    /**
     * Starts a dialog
     * @param List<string>  dialog                                  An list of sentences to display
     * @param callback      onDialogCloseCallback   (optionnal)     A callback function once the last sentence has been skiped
     * @param bool          isChained               (optionnal)     True if called inside a process involving player's control (like a cinematic)
     */
    public void StartDialog(List<string> dialog, onDialogClose onDialogCloseCallback = null, bool isChained = false)
    {
        onDialogClose onDialogClose = null;
        bool hasNpcMovementManager = GetComponentInParent<NPCMovementManager>() != null;
        DialogManager.isReading = true;

        if (hasNpcMovementManager)
        {
            GetComponentInParent<NPCMovementManager>().InterruptMoving();
        }

        onDialogClose = delegate () {
            if (!isChained)
            {
                playerController.UnrestrainPlayer();
            }
            
            if (hasNpcMovementManager)
            {
                transform.parent.GetComponent<NPCMovementManager>().StartMovingRandomly();
            }
            DialogManager.isReading = false;
            if (onDialogCloseCallback != null)
            {
                onDialogCloseCallback();
            }
        };

        if (!isChained)
        {
            playerController.RestrainPlayer();
        }
           
        dialogManager.OpenDialog(dialog, onDialogClose);
    }

    public void StartOwnDialog(bool isChained = false)
    {
        StartDialog(dialogue, onDialogCloseCallback, isChained);
    }
}
