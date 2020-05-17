using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void onDialogClose();

public class DialogManager : MonoBehaviour, IGameLoadListener
{
    public static bool isReading;

    public GameObject dialogBox;
    public Text dialogText;
    public Text bottomText;

    public bool IsDialogActive { get; set; }
    protected List<string> texts;
    protected int currentTextIndex;

    protected KeyEventController keyEventController;

    // callbacks
    protected onDialogClose onDialogCloseCallback;

    // Use this for initialization
    void Start () {
        keyEventController = FindObjectOfType<KeyEventController>();
        keyEventController.WatchKeyCode(KeyCode.Space);
        ResetTexts();
    }
	
	// Update is called once per frame
	void Update () {
        if (IsDialogActive && keyEventController.IsKeyPressed(KeyCode.Space))
        {
            keyEventController.AllocateKeyPress(KeyCode.Space);
            DisplayNextOrClose();
        }
    }

    private void FixedUpdate()
    {
        
    }

    public void OpenDialog(string text, onDialogClose dialogCloseCallback = null)
    {
        onDialogCloseCallback = dialogCloseCallback;
        texts.Add(text);
        _OpenDialog();
    }

    public void OpenDialog(List<string> text, onDialogClose dialogCloseCallback = null)
    {
        onDialogCloseCallback = dialogCloseCallback;
        texts = text;
        _OpenDialog();
    }

    public void _OpenDialog()
    {
        currentTextIndex = -1;
        dialogBox.SetActive(true);
        IsDialogActive = true;
        DisplayNextOrClose();
    }

    public void ResetTexts()
    {
        texts = new List<string>();
    }

    public bool HasNextText()
    {
        return currentTextIndex < texts.Count - 1; 
    }

    public void DisplayNextOrClose()
    {
        if (HasNextText())
        {
            DisplayNextText();
        }
        else
        {
            CloseDialog();

            if (onDialogCloseCallback != null)
            {
                onDialogCloseCallback();
            }
        }
    }

    public void DisplayNextText()
    {
        currentTextIndex++;
        string nextText = texts[currentTextIndex];
        DisplayText(nextText);
    }

    public void DisplayText(string text)
    {
        dialogText.text = text;
    }

    public void CloseDialog()
    {
        dialogText.text = "";
        dialogBox.SetActive(false);
        IsDialogActive = false;
        ResetTexts();
    }

    public void OnGameLoad()
    {
        CloseDialog(); // closes all dialogs
    }
}
