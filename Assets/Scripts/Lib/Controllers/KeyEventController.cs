using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEventController : MonoBehaviour
{
    protected float keyPressResetTime = 0.5f;
    protected float dialogSpamPreventingTimeCounter;

    protected Dictionary<KeyCode, bool> pressedKeys = new Dictionary<KeyCode, bool>(); // the bool represents if the pressedKey is available for use

    // Use this for initialization
    protected void Start()
    {
       
    }

    // Update is called once per frame
    protected void Update()
    {
        HandleKeyPressEvents();
    }

    protected void HandleKeyPressEvents()
    {
        KeyCode spaceKeyCode = KeyCode.Space;

        if (Input.GetKeyDown(spaceKeyCode))
        {
            if (!pressedKeys.ContainsKey(spaceKeyCode))
            {
                pressedKeys.Add(spaceKeyCode, true);
                StartCoroutine(KeyPressResetTimer(spaceKeyCode));
            }
        }

        if (Input.GetKeyUp(spaceKeyCode))
        {
            pressedKeys.Remove(spaceKeyCode);
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
