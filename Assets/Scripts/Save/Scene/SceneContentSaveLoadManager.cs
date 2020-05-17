using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneContentSaveLoadManager : AbstractSaveLoadManager
{
    protected override void Start()
    {
        base.Start();
        entityManager = FindObjectOfType<CurrentSceneManager>();
    }

    public override ObjectData GetObjectData(MonoBehaviour entityManager)
    {
        return new SceneContentData((CurrentSceneManager) entityManager);
    }

    public override void RebuildFromData(ObjectData objectData)
    {
        SceneContentData currentSceneContentData = (SceneContentData) objectData;
        CurrentSceneManager currentSceneManager = (CurrentSceneManager) entityManager;

        CallAfterSceneLoaded(delegate ()
        {
            RebuildTransformSaveHolders(currentSceneContentData, currentSceneManager);
            RebuildMovables(currentSceneContentData, currentSceneManager);
            RebuildEnnemies(currentSceneContentData, currentSceneManager);
        });
    }

    public void RebuildTransformSaveHolders(SceneContentData currentSceneContentData, CurrentSceneManager currentSceneManager)
    {
        foreach (SceneContentData.TransformSaveHolderData transformSaveHolderData in currentSceneContentData.transformSaveHoldersData)
        {
            foreach (MonoBehaviour transformSaveHolder in currentSceneManager.GetTransformSaveHolders())
            {
                if (Helper.GetObjectLocalIdInFile(transformSaveHolder) == transformSaveHolderData.localIdInFile)
                {
                    PrepareDataForEntity(transformSaveHolder, delegate ()
                    {
                        transformSaveHolder.transform.position = new Vector2(
                            transformSaveHolderData.position[0],
                            transformSaveHolderData.position[1]
                        );

                        transformSaveHolder.transform.rotation = new Quaternion(
                            transformSaveHolderData.rotation[0],
                            transformSaveHolderData.rotation[1],
                            transformSaveHolderData.rotation[2],
                            transformSaveHolderData.rotation[3]
                            );

                        transformSaveHolder.gameObject.SetActive(transformSaveHolderData.isActive);
                    });
                }
            }
        }
    }

    public void RebuildMovables(SceneContentData currentSceneContentData, CurrentSceneManager currentSceneManager)
    {
        foreach (SceneContentData.MovableData movableData in currentSceneContentData.movablesData)
        {
            foreach (Movable movable in currentSceneManager.GetMovableObjects())
            {
                if (Helper.GetObjectLocalIdInFile(movable) == movableData.localIdInFile)
                {
                    PrepareDataForEntity(movable, delegate ()
                    {
                        movable.moveSpeed = movableData.moveSpeed;
                        movable.IsMoving = movableData.IsMoving;
                        movable.AreMovementsRestrained = movableData.AreMovementsRestrained;
                        movable.IsImmobilized = movableData.IsImmobilized;
                        movable.IsInCinematicMode = movableData.IsInCinematicMode;

                        movable.LastMove = new Vector3
                            (movableData.LastMove[0],
                            movableData.LastMove[1],
                            movableData.LastMove[2]
                            );

                        movable.MoveInput = new Vector3
                            (movableData.MoveInput[0],
                            movableData.MoveInput[1],
                            movableData.MoveInput[2]
                            );
                    });
                }
            }
        }
    }

    public void RebuildAutoMovings(SceneContentData currentSceneContentData, CurrentSceneManager currentSceneManager)
    {
        foreach (SceneContentData.AutoMovingData autoMovingData in currentSceneContentData.autoMovingsData)
        {
            foreach (AutoMoving autoMoving in currentSceneManager.GetMovableObjects())
            {
                if (Helper.GetObjectLocalIdInFile(autoMoving) == autoMovingData.localIdInFile)
                {
                    PrepareDataForEntity(autoMoving, delegate ()
                    {
                        autoMoving.averageMoveTime = autoMovingData.averageMoveTime;
                        autoMoving.averageMoveTimeDelta = autoMovingData.averageMoveTimeDelta;
                        autoMoving.betweenMovesAverageTimeDelta = autoMovingData.betweenMovesAverageTimeDelta;
                        autoMoving.moveDirectionType = (AutoMoving.AutomoveDirectionType) autoMovingData.moveDirectionType;
                        autoMoving.TimeBetweenMovesCounter = autoMovingData.TimeBetweenMovesCounter;
                        autoMoving.MoveTimeCounter = autoMovingData.MoveTimeCounter;
                        autoMoving.CanMove = autoMovingData.CanMove;
                    });
                }
            }
        }
    }

    public void RebuildEnnemies(SceneContentData currentSceneContentData, CurrentSceneManager currentSceneManager)
    {
        foreach (SceneContentData.EnnemyData ennemyData in currentSceneContentData.ennemiesData)
        {
            foreach (EnnemyController ennemyController in currentSceneManager.GetEnnemies())
            {
                if (Helper.GetObjectLocalIdInFile(ennemyController) == ennemyData.localIdInFile)
                {
                    PrepareDataForEntity(ennemyController, delegate ()
                    {
                        ennemyController.averageMoveTime = ennemyData.averageMoveTime;
                        ennemyController.betweenMovesAverageTime = ennemyData.betweenMovesAverageTime;
                        ennemyController.TimeBetweenMovesCounter = ennemyData.TimeBetweenMovesCounter;
                        ennemyController.MoveTimeCounter = ennemyData.MoveTimeCounter;
                    });
                }
            }
        }
    }

    [Serializable]
    public class SceneContentData : ObjectData
    {
        public TransformSaveHolderData[] transformSaveHoldersData;
        public MovableData[] movablesData;
        public AutoMovingData[] autoMovingsData;
        public EnnemyData[] ennemiesData;

        public SceneContentData(CurrentSceneManager currentSceneManager)
        {
            List<MonoBehaviour> transformSaveHolders = currentSceneManager.GetTransformSaveHolders();
            transformSaveHoldersData = new TransformSaveHolderData[transformSaveHolders.Count];
            int i = 0;

            foreach (MonoBehaviour transformSaveHolder in transformSaveHolders)
            {
                transformSaveHoldersData[i] = new TransformSaveHolderData(transformSaveHolder);
                i++;
            }

            Movable[] movables = currentSceneManager.GetMovableObjects();
            movablesData = new MovableData[movables.Length];
            i = 0;

            foreach (Movable movable in movables)
            {
                movablesData[i] = new MovableData(movable);
                i++;
            }

            AutoMoving[] autoMovings = currentSceneManager.GetAutoMovingObjects();
            autoMovingsData = new AutoMovingData[autoMovings.Length];
            i = 0;

            foreach (AutoMoving autoMoving in autoMovings)
            {
                autoMovingsData[i] = new AutoMovingData(autoMoving);
                i++;
            }

            EnnemyController[] ennemies = currentSceneManager.GetEnnemies();
            ennemiesData = new EnnemyData[ennemies.Length];
            i = 0;

            foreach (EnnemyController movable in ennemies)
            {
                ennemiesData[i] = new EnnemyData(movable);
                i++;
            }
        }

        [Serializable]
        public class TransformSaveHolderData
        {
            public long localIdInFile;
            public float[] position = new float[2];
            public float[] rotation = new float[4];
            public bool isActive;

            public TransformSaveHolderData(MonoBehaviour transformSaveHolder)
            {
                localIdInFile = Helper.GetObjectLocalIdInFile(transformSaveHolder);

                position[0] = transformSaveHolder.transform.position.x;
                position[1] = transformSaveHolder.transform.position.y;

                rotation[0] = transformSaveHolder.transform.rotation.x;
                rotation[1] = transformSaveHolder.transform.rotation.y;
                rotation[2] = transformSaveHolder.transform.rotation.z;
                rotation[3] = transformSaveHolder.transform.rotation.w;

                isActive = transformSaveHolder.gameObject.activeSelf;
            }
        }

        [Serializable]
        public class MovableData
        {
            public long localIdInFile;
            public float moveSpeed;
            public bool IsMoving;
            public float[] LastMove = new float[3];
            public bool AreMovementsRestrained;
            public bool IsImmobilized;
            public float[] MoveInput = new float[3];
            public bool IsInCinematicMode;

            public MovableData(Movable movable)
            {
                localIdInFile = Helper.GetObjectLocalIdInFile(movable);

                moveSpeed = movable.moveSpeed;
                IsMoving = movable.IsMoving;
                LastMove[0] = movable.LastMove.x;
                LastMove[1] = movable.LastMove.y;
                LastMove[2] = movable.LastMove.y;
                AreMovementsRestrained = movable.AreMovementsRestrained;
                IsImmobilized = movable.IsImmobilized;
                MoveInput[0] = movable.MoveInput.x;
                MoveInput[1] = movable.MoveInput.y;
                MoveInput[2] = movable.MoveInput.y;
                IsInCinematicMode = movable.IsInCinematicMode;
            }
        }

        [Serializable]
        public class AutoMovingData
        {
            public long localIdInFile;
            public float averageMoveTime;
            public float averageMoveTimeDelta;
            public float betweenMovesAverageTimeDelta;
            public int moveDirectionType;
            public float TimeBetweenMovesCounter;
            public float MoveTimeCounter;
            public bool CanMove;

            public AutoMovingData(AutoMoving autoMoving)
            {
                localIdInFile = Helper.GetObjectLocalIdInFile(autoMoving);

                averageMoveTime = autoMoving.averageMoveTime;
                averageMoveTimeDelta = autoMoving.averageMoveTimeDelta;
                betweenMovesAverageTimeDelta = autoMoving.betweenMovesAverageTimeDelta;
                moveDirectionType = Convert.ToInt32(autoMoving.moveDirectionType);
                TimeBetweenMovesCounter = autoMoving.TimeBetweenMovesCounter;
                MoveTimeCounter = autoMoving.MoveTimeCounter;
                CanMove = autoMoving.CanMove;
            }
        }

        [Serializable]
        public class EnnemyData
        {
            public long localIdInFile;
            public float averageMoveTime;
            public float betweenMovesAverageTime;

            public float TimeBetweenMovesCounter;
            public float MoveTimeCounter;

            public EnnemyData(EnnemyController ennemyController)
            {
                localIdInFile = Helper.GetObjectLocalIdInFile(ennemyController);

                averageMoveTime = ennemyController.averageMoveTime;
                betweenMovesAverageTime = ennemyController.betweenMovesAverageTime;
                TimeBetweenMovesCounter = ennemyController.TimeBetweenMovesCounter;
                MoveTimeCounter = ennemyController.MoveTimeCounter;
            }
        }
    }
}
