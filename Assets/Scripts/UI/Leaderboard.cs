using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private Transform _entryPrefab;
    [SerializeField] private Transform _spawnDestination;
    

    public void LoadLeaderboard(List<LeaderboardEntry> _toLoad)
    {
        for (int i = _toLoad.Count-1; i > 0; i--)
        {
            SpawnRow(_toLoad[i].GetPosition(), _toLoad[i].GetName(), _toLoad[i].GetScore());
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SpawnRow(int Position, string DName, int Score)
    {
        Transform obj = Instantiate(_entryPrefab, _spawnDestination);
        
        obj.GetComponent<LeaderboardEntrySetup>().SetupEntry(Position, DName, Score);
    }
}
