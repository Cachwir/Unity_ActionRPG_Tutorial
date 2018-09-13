using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicSceneObjectMoveBuilder : AbstractCinematicObjectMoveBuilder
{
    public GameObject targetObject;
    public bool isZPositionFixed = true; // If true, the z position of this gameObject will be fixed (in order to avoid some bugs)
    public float zPositionFixedValue = 0; // The value the z position will be fixed to

    void Start()
    {
        target = targetObject;
        _isZPositionFixed = isZPositionFixed;
        _zPositionFixedValue = zPositionFixedValue;
    }
}
