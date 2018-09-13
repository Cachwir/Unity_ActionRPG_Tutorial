using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterKillQuest : Quest {
    
    public bool IsMonsterDead { get; set; }
    public EnnemyHealthManager targetEnnemy;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void StartQuest()
    {
        base.StartQuest();
        targetEnnemy.SubscribeEnnemyKilledEvent(ReportMonsterDeath);
    }

    public void ReportMonsterDeath()
    {
        IsMonsterDead = true;
    }

    public override SerializableQuest ExtractSerializableData()
    {
        return new SerializableMonsterQuest(this);
    }

    public void RebuildFromSerializableQuest(SerializableMonsterQuest serializableQuest)
    {
        base.RebuildFromSerializableQuest(serializableQuest);

        IsMonsterDead = serializableQuest.IsMonsterDead;
    }

    [Serializable]
    public class SerializableMonsterQuest : SerializableQuest
    {
        public bool IsMonsterDead;

        public SerializableMonsterQuest(MonsterKillQuest quest) : base(quest)
        {
            IsMonsterDead = quest.IsMonsterDead;
        }
    }
}
