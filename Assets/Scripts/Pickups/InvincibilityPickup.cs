using UnityEngine;

public class InvincibilityPickup : Pickup
{
    //Uses the character interface to trigger invincibility event
    private void OnTriggerEnter(Collider other)
    {
        ICharacter charScript = other.GetComponent<ICharacter>();
        if (charScript == null)
        {
            return;
        }
        charScript.Invincibility();
        Deactivate();
    }
}