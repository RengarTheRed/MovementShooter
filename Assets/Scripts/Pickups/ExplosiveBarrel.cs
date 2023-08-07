using System;
using Unity.VisualScripting;
using UnityEngine;

class ExplosiveBarrel : Pickup
{
    private bool _blownUp = false;
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody _rb = other.GetComponent<Rigidbody>();
        if (_rb == null)
        {
            return;
        }

        if(_blownUp==false)
        {
            if (_rb.velocity.magnitude > 30)
            {
                _blownUp = true;
                RaycastHit[] toDamage = Physics.SphereCastAll(this.transform.position, 15f, transform.forward);
                foreach (var hit in toDamage)
                {
                    if (hit.collider.isTrigger)
                    {
                        ICharacter charHit = hit.transform.GetComponent<ICharacter>();
                        if (charHit!=null)
                        {
                            charHit.TakeDamage(5);
                            Debug.Log("Did damage to " + hit.transform.name);
                        }
                    }
                }
            }
            Deactivate();
        }
    }
}
