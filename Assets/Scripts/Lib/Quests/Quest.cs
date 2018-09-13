using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Quest : MonoBehaviour {
    
    public string _name;
    public string _slug;
    public string _description;

    public bool _IsStarted { get; set; }
    public bool _IsFinished { get; set; }
    public bool _IsCompleted { get; set; }
    public bool _IsFailed { get; set; }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool CanStartQuest()
    {
        return !_IsStarted && !_IsFinished;
    }

    public virtual void StartQuest()
    {
        _IsStarted = true;
    }

    public void EndQuestWithSuccess()
    {
        _IsFinished = true;
        _IsCompleted = true;
    }

    public void EndQuestWithFailure()
    {
        _IsFinished = true;
        _IsFailed = true;
    }

    public bool IsActive()
    {
        return _IsStarted && !_IsFinished;
    }

    public virtual SerializableQuest ExtractSerializableData()
    {
        return new SerializableQuest(this);
    }

    public void RebuildFromSerializableQuest(SerializableQuest serializableQuest)
    {
        _IsStarted = serializableQuest._IsStarted;
        _IsFinished = serializableQuest._IsFinished;
        _IsCompleted = serializableQuest._IsCompleted;
        _IsFailed = serializableQuest._IsFailed;
    }

    [Serializable]
    public class SerializableQuest
    {
        public long localIdInFile;

        public bool _IsStarted;
        public bool _IsFinished;
        public bool _IsCompleted;
        public bool _IsFailed;

        public SerializableQuest(Quest quest)
        {
            localIdInFile = Helper.GetObjectLocalIdInFile(quest);
            _IsStarted = quest._IsStarted;
            _IsFinished = quest._IsFinished;
            _IsCompleted = quest._IsCompleted;
            _IsFailed = quest._IsFailed;
        }
    }
}
