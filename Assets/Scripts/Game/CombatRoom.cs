using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CombatRoom : RoomManager
    {
        public List<Enemy> _RoomEnemies;
        
        //Function accessed by enemy when dies in room
        public void OnEnemyKilled(Enemy enemyKilled)
        {
            if (CheckAndDropFromList(enemyKilled))
            {
                //Check all enemies killed
                if (_RoomEnemies.Count == 0)
                {
                    ClearRoom();
                }
            }
        }
        //Checks if Enemy exists in room and drops from list
        private bool CheckAndDropFromList(Enemy enemyKilled)
        {
            if (_RoomEnemies.Contains(enemyKilled))
            {
                _RoomEnemies.Remove(enemyKilled);
                return true;
            }
            Debug.Log("Room does not contain " + enemyKilled.name);
            return false;
        }
    }
}