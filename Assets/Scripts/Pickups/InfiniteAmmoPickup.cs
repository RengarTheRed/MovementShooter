using UnityEngine;

public class InfiniteAmmoPickup : Pickup
{
    //Uses the Character Interface heal function to restore hp
    private void OnTriggerEnter(Collider other)
    {
        ICharacter charScript = other.GetComponent<ICharacter>();
        if (charScript == null)
        {
            return;
        }
        charScript.InfiniteAmmo();
        Deactivate();
    }
}