using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class StepStatesSaveLoadManager : AbstractSaveLoadManager
{
    protected override void Start()
    {
        base.Start();
        entityManager = FindObjectOfType<StepStatesManager>();
    }

    public override ObjectData GetObjectData(MonoBehaviour entityManager)
    {
        return new StepStatesData((StepStatesManager) entityManager);
    }

    public override void RebuildFromData(ObjectData objectData)
    {
        StepStatesData stepStatesData = (StepStatesData) objectData;
        StepStatesManager stepStatesManager = (StepStatesManager)entityManager;

        CallAfterSceneLoaded(delegate ()
        {
            PrepareDataForEntity(stepStatesManager, delegate ()
            {
                for (int i = 0, c = stepStatesData.keys.Length; i < c; i++)
                {
                    stepStatesManager.CompletedStepStates[stepStatesData.keys[i]] = stepStatesData.values[i]; // stores the step state value
                }

                // useless since the scene is reloaded
                // stepStatesManager.ReloadAllStepStatesHolderState(); // rechecks all step states in the current scene
            });
        }); 
    }

    [Serializable]
    public class StepStatesData : ObjectData
    {
        public long[] keys;
        public string[] values;

        public StepStatesData(StepStatesManager stepStatesManager)
        {
            keys = new long[stepStatesManager.CompletedStepStates.Count];
            values = new string[stepStatesManager.CompletedStepStates.Count];

            int i = 0;

            foreach (KeyValuePair<long, string> pair in stepStatesManager.CompletedStepStates)
            {
                keys[i] = pair.Key;
                values[i] = pair.Value;
                i++;
            }
        }
    }
}
