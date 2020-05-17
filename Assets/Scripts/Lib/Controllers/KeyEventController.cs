using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEventController : MonoBehaviour
{
    protected float keyPressResetTime = 0.5f;
    protected float dialogSpamPreventingTimeCounter;

    protected List<KeyCode> watchedKeyCodes;
    protected Dictionary<KeyCode, bool> pressedKeys = new Dictionary<KeyCode, bool>(); // the bool represents if the pressedKey is available for use

    // Use this for initialization
    protected void Start()
    {
        watchedKeyCodes = new List<KeyCode>();
    }

    // Update is called once per frame
    protected void Update()
    {
        HandleKeyPressEvents();
    }

    public void WatchKeyCode(KeyCode keyCode)
    {
        if (!watchedKeyCodes.Contains(keyCode))
        {
            watchedKeyCodes.Add(keyCode);
        }
    }

    protected void HandleKeyPressEvents()
    {
        foreach (KeyCode keyCode in watchedKeyCodes)
        {
            if (Input.GetKeyDown(keyCode))
            {
                if (!pressedKeys.ContainsKey(keyCode))
                {
                    pressedKeys.Add(keyCode, true);
                    StartCoroutine(KeyPressResetTimer(keyCode));
                }
            }

            if (Input.GetKeyUp(keyCode))
            {
                pressedKeys.Remove(keyCode);
            }
        }
    }
    
    public bool IsKeyPressed(KeyCode keyCode)
    {
        return GetValue(keyCode);
    }

    public void AllocateKeyPress(KeyCode keyCode)
    {
        pressedKeys[keyCode] = false;
    }

    protected bool GetValue(KeyCode keyCode)
    {
        bool isKeyPressed = false;
        pressedKeys.TryGetValue(keyCode, out isKeyPressed);

        return isKeyPressed;
    }

    IEnumerator KeyPressResetTimer(KeyCode keyCode)
    {
        yield return new WaitForSeconds(keyPressResetTime);

        pressedKeys.Remove(keyCode);
    }
}
