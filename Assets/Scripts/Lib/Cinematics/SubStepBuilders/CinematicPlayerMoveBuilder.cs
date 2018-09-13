using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicPlayerMoveBuilder : AbstractCinematicObjectMoveBuilder
{
    void Start()
    {
        target = FindObjectOfType<PlayerController>().gameObject;
    }
}
