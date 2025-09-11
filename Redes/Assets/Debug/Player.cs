using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkTransform))]
public class Player : NetworkBehaviour
{
    [SerializeField] float _speed;
    public override void FixedUpdateNetwork()
    {
        var dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (dir != Vector3.zero)
        {
            Movement(dir);
        }
    }

    void Movement(Vector3 dir)
    {
        transform.position += Vector3.ClampMagnitude(dir, 1) * (_speed * Runner.DeltaTime);

    }
}
