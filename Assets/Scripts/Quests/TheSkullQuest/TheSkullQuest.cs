using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CallBack();

public class TheSkullQuest : Quest {
    
    public bool IsSkullRetrieved { get; set; }

    protected PlayerController playerController;

    // Use this for initialization
    void Start () {
        _name = "The Skull Quest";
        _description = "Go retrieve dat skull!";

        playerController = FindObjectOfType<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RetrieveSkull(QuestStepDialogHolder dialogHolder)
    {
        dialogHolder.transform.parent.gameObject.SetActive(false);
        IsSkullRetrieved = true;
    }

    public void EndQuestSpecial(QuestStepDialogHolder dialogHolder)
    {
        dialogHolder.transform.parent.gameObject.SetActive(false);
        playerController.RestrainPlayer();
        StartCoroutine(Wait(2, delegate ()
        {
            dialogHolder.StartDialog(new List<string>() { "...", "(On dirait qu'il a oublié pour la récompense)" }, delegate() {
                playerController.UnrestrainPlayer();
                EndQuestWithSuccess();
            });
        }));
        
    }

    IEnumerator Wait(float time, CallBack callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

    public override SerializableQuest ExtractSerializableData()
    {
        return new SerializableTheSkullQuest(this);
    }

    public void RebuildFromSerializableQuest(SerializableTheSkullQuest serializableQuest)
    {
        base.RebuildFromSerializableQuest(serializableQuest);

        IsSkullRetrieved = serializableQuest.IsSkullRetrieved;
    }

    [Serializable]
    public class SerializableTheSkullQuest : SerializableQuest
    {
        public bool IsSkullRetrieved;

        public SerializableTheSkullQuest(TheSkullQuest quest) : base(quest)
        {
            IsSkullRetrieved = quest.IsSkullRetrieved;
        }
    }
}
