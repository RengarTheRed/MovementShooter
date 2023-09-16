using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Task that fires the held gun by the gameobject")]
    [TaskIcon("{SkinColor}LogIcon.png")]
    public class FireGun : Action
    {
        [Tooltip("NPC to fire Gun")]
        new public SharedGameObject gameObject;

        public override TaskStatus OnUpdate()
        {
            GunScript gun = gameObject.Value.GetComponentInChildren<GunScript>();
            gun.PublicFire();
            return TaskStatus.Success;
        }
    }
}