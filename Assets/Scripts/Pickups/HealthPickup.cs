using UnityEngine;

public class HealthPickup : Pickup
{
    //Uses the Character Interface heal function to restore hp
    private void OnTriggerEnter(Collider other)
    {
        ICharacter charScript = other.GetComponent<ICharacter>();
        if (charScript == null)
        {
            return;
        }
        charScript.Heal(10);
        Deactivate();
    }
}