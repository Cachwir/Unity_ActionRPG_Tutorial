using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {

    public float moveSpeed;
    public string text;
    public Text displayedText;

	// Use this for initialization
	void Start () {
        displayedText.text = text;
    }
	
	// Update is called once per frame
	void Update () {
	}

    private void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + (moveSpeed * Time.fixedDeltaTime));
    }
}
