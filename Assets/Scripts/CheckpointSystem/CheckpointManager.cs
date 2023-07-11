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
        var sortedPoints = _checkPoints.OrderBy(q => q._checkPointID).ToList();
        _checkPoints = sortedPoints;
    }

    private void CompareSpots(CheckPoint cP1, CheckPoint cP2)
    {
        if (cP1._checkPointID > cP2._checkPointID)
        {
            CheckPoint tmp = cP2;
            cP2._checkPointID = cP1._checkPointID;
            cP1._checkPointID = tmp._checkPointID;
        }
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
        return _checkPoints[inID].transform;
    }
}
