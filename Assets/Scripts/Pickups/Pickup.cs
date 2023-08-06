using System;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Transform _mesh;

    private void FixedUpdate()
    {
        _mesh.Rotate(0,1,0);
    }

    protected void Deactivate()
    {
        Destroy(this.gameObject);
    }
}