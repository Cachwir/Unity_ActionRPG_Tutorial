using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSaveLoadManager : AbstractSaveLoadManager
{
    protected override void Start()
    {
        base.Start();
        entityManager = FindObjectOfType<PlayerController>();
    }

    public override ObjectData GetObjectData(MonoBehaviour entityManager)
    {
        return new PlayerData((PlayerController) entityManager);
    }

    public override void RebuildFromData(ObjectData objectData)
    {
        PlayerData playerData = (PlayerData) objectData;
        PlayerController currentPlayerController = (PlayerController) entityManager;
        PlayerHealthManager currentPlayerHealthManager = entityManager.GetComponent<PlayerHealthManager>();

        CallAfterSceneLoaded(delegate ()
        {
            PrepareDataForEntity(currentPlayerController, delegate ()
            {
                currentPlayerController.LastMove = new Vector2(playerData.playerControllerData.lastMove[0], playerData.playerControllerData.lastMove[1]);
                currentPlayerController.AreMovementsRestrained = playerData.playerControllerData.AreMovementsRestrained;
                currentPlayerController.IsImmobilized = playerData.playerControllerData.IsImmobilized;
                currentPlayerController.IsInCinematicMode = playerData.playerControllerData.IsInCinematicMode;
                currentPlayerController.AreActionsRestrained = playerData.playerControllerData.AreActionsRestrained;
                currentPlayerController.IsPlayerRestrained = playerData.playerControllerData.IsPlayerRestrained;

                currentPlayerHealthManager.StopBlinking();
                currentPlayerHealthManager.playerMaxHealth = playerData.playerHealthData.playerMaxHealth;
                currentPlayerHealthManager.PlayerCurrentHealth = playerData.playerHealthData.PlayerCurrentHealth;
            });
        });
            
    }

    [Serializable]
    public class PlayerData : ObjectData
    {
        public PlayerControllerData playerControllerData;
        public PlayerHealthData playerHealthData;
        
        public PlayerData(PlayerController playerController)
        {
            PlayerHealthManager playerHealthManager = playerController.GetComponent<PlayerHealthManager>();

            playerControllerData = new PlayerControllerData(playerController);
            playerHealthData = new PlayerHealthData(playerHealthManager);
        }

        [Serializable]
        public class PlayerControllerData
        {
            // public float[] position = new float[2]; // already taken in charge by the SceneSaveLoadManager
            public float[] lastMove = new float[2];
            
            public bool AreMovementsRestrained;
            public bool IsImmobilized;
            public bool IsInCinematicMode;
            public bool AreActionsRestrained;
            public bool IsPlayerRestrained;

            public PlayerControllerData(PlayerController playerController)
            {
                lastMove[0] = playerController.LastMove.x;
                lastMove[1] = playerController.LastMove.y;
                
                AreMovementsRestrained = playerController.AreMovementsRestrained;
                IsImmobilized = playerController.IsImmobilized;
                IsInCinematicMode = playerController.IsInCinematicMode;
                AreActionsRestrained = playerController.AreActionsRestrained;
                IsPlayerRestrained = playerController.IsPlayerRestrained;
            }
        }

        [Serializable]
        public class PlayerHealthData
        {
            public int playerMaxHealth;
            public int PlayerCurrentHealth;

            public PlayerHealthData(PlayerHealthManager playerHealthManager)
            {
                playerMaxHealth = playerHealthManager.playerMaxHealth;
                PlayerCurrentHealth = playerHealthManager.PlayerCurrentHealth;
            }
        }
    }
}
