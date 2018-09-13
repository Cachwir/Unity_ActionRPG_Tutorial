using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartCallbackCaller : MonoBehaviour {
    
	
	// Update is called once per frame
	void Update () {
        EntitiesOnStartCallbackManager.CallAll();
        Destroy(this.gameObject);
    }
}
