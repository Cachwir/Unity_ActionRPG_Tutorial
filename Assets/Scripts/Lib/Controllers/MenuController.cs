using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public GameObject menu;

    protected GameManager gameManager;
    protected KeyEventController keyEventController;
    protected KeyCode menuToggleKey = KeyCode.Escape;

	// Use this for initialization
	void Start ()
    {
        gameManager = FindObjectOfType<GameManager>();
        keyEventController = FindObjectOfType<KeyEventController>();
        keyEventController.WatchKeyCode(menuToggleKey);
    }
	
	// Update is called once per frame
	void Update ()
    {
        HandleEchapKeyPress();

    }

    void HandleEchapKeyPress()
    {
        if (keyEventController.IsKeyPressed(menuToggleKey))
        {
            keyEventController.AllocateKeyPress(menuToggleKey);
            ToggleMenu();
        }
    }

    public bool IsMenuOpened()
    {
        return menu.activeSelf;
    }

    public void OpenMenu()
    {
        gameManager.PauseGame();
        menu.SetActive(true);
    }

    public void CloseMenu()
    {
        menu.SetActive(false);
        gameManager.UnpauseGame();
    }

    public void ToggleMenu()
    {
        if (IsMenuOpened())
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }
}
