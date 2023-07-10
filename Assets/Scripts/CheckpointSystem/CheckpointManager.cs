using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Player _Player;
    // Start is called before the first frame update
    void Start()
    {
        _Player = FindFirstObjectByType<Player>();
    }

    public void CompareCheckPoint(int inID)
    {
        if (inID > _Player.GetCheckPoint())
        {
            _Player.SetCheckPoint(inID);
        }
    }
}
