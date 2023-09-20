using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private Transform _entryPrefab;
    [SerializeField] private Transform _spawnDestination;

    private int posID = 0;
    
    private void Awake()
    {
        LoadLeaderboard();
    }

    public void LoadLeaderboard()
    {
        for (int i = 0; i < 23; i++)
        {
            SpawnRow(i+1, "John", 5);
        }
    }

    private void SpawnRow(int Position, string DName, int Score)
    {
        Transform obj = Instantiate(_entryPrefab, _spawnDestination);
        
        obj.GetComponent<LeaderboardEntrySetup>().SetupEntry(Position, DName, Score);
    }
}
