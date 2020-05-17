using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour, IGameLoadListener, IOnLevelUnload
{
    public string CombatMusicTag;

    protected MusicController musicController;

    public bool IsInCombat { get; set; }

    protected List<EnnemyController> ennemies = new List<EnnemyController>();

    // Use this for initialization
    void Start ()
    {
        musicController = FindObjectOfType<MusicController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        if (IsInCombat && !IsThereEnnemiesInCombat())
        {
            LeaveCombatMode();
        }
    }

    public void RegisterEnnemy(EnnemyController ennemy)
    {
        ennemies.Add(ennemy);
    }

    public bool IsThereEnnemiesInCombat()
    {
        return ennemies.Find(x => x.IsInCombat) != null;
    }

    public void OnLevelUnload()
    {
        LeaveCombatMode();
        ennemies.Clear();
    }

    public void OnGameLoad()
    {
        OnLevelUnload();
    }

    public void NotifyEnterCombat(EnnemyController ennemy)
    {
        if (!IsInCombat)
        {
            EnterCombatMode();
        }
    }

    public void EnterCombatMode()
    {
        musicController.SwitchToTrack(CombatMusicTag, false);
        IsInCombat = true;
    }

    public void LeaveCombatMode()
    {
        if (IsInCombat)
        {
            musicController.SwitchToPreviousTrack();
        }
        
        IsInCombat = false;
    }
}
