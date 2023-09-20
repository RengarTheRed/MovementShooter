using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private Transform _entryPrefab;
    private void Awake()
    {
        LoadLeaderboard();
    }

    public void LoadLeaderboard()
    {
        
    }
}
