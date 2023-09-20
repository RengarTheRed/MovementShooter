using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardEntrySetup : MonoBehaviour
{
    [SerializeField] private TMP_Text _Name;
    [SerializeField] private TMP_Text _Score;
    [SerializeField] private TMP_Text _Position;

    public void SetupEntry(int Position, string DName, int Score)
    {
        _Name.text = DName;
        _Position.text = Position.ToString();
        _Score.text = Score.ToString();
    }
}