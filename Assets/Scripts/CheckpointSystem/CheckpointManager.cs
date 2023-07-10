using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CheckpointSystem;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Player _Player;

    private List<CheckPoint> _checkPoints;
    // Start is called before the first frame update
    void Start()
    {
        _Player = FindFirstObjectByType<Player>();
        GetCheckPoints();
    }

    //Need to get and sort using their own ID instead
    private void GetCheckPoints()
    {
        _checkPoints = FindObjectsByType<CheckPoint>(FindObjectsSortMode.InstanceID).ToList();
    }

    public void CompareCheckPoint(int inID)
    {
        if (inID > _Player.GetCheckPoint())
        {
            _Player.SetCheckPoint(inID);
        }
    }

    public Transform GetPlayerCheckPoint(int inID)
    {
        return _checkPoints[inID+1].transform;
    }
}
