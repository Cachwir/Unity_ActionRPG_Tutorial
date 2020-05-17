using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour {

    public float timeBeforeDestroyed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        timeBeforeDestroyed -= Time.fixedDeltaTime;

        if (timeBeforeDestroyed <= 0)
        {
            Destroy(gameObject);
        }
    }
}
