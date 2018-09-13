using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsSaveLoadManager : AbstractSaveLoadManager
{
    protected override void Start()
    {
        base.Start();
        entityManager = FindObjectOfType<QuestManager>();
    }

    public override ObjectData GetObjectData(MonoBehaviour entityManager)
    {
        return new QuestsData((QuestManager) entityManager);
    }

    public override void RebuildFromData(ObjectData objectData)
    {
        QuestsData questsData = (QuestsData)objectData;
        QuestManager questManager = (QuestManager) entityManager;

        CallAfterSceneLoaded(delegate ()
        {
            foreach (Quest quest in questManager.GetQuests())
            {
                foreach (Quest.SerializableQuest serializedQuest in questsData.quests)
                {
                    if (serializedQuest.localIdInFile == Helper.GetObjectLocalIdInFile(quest))
                    {
                        PrepareDataForEntity(quest, delegate ()
                        {
                            quest.RebuildFromSerializableQuest(serializedQuest);
                        });
                    }
                }
            }
        }); 
    }

    [Serializable]
    public class QuestsData : ObjectData
    {
        public Quest.SerializableQuest[] quests;

        public QuestsData(QuestManager questManager)
        {
            quests = new Quest.SerializableQuest[questManager.GetQuests().Count];
            int i = 0;

            foreach (Quest quest in questManager.GetQuests())
            {
                quests[i] = quest.ExtractSerializableData();
                i++;
            }
        }
    }
}
