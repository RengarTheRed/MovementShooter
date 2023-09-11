using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Task that fires the held gun by the gameobject")]
    [TaskIcon("{SkinColor}LogIcon.png")]
    public class FireGun : Action
    {
        [Tooltip("NPC to fire Gun")]
        public GameObject gameObject;

        public override TaskStatus OnUpdate()
        {
            gameObject.GetComponentInChildren<GunScript>().PublicFire();
            return TaskStatus.Success;
        }
    }
}