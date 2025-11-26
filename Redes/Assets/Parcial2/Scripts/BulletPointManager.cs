using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPointManager : NetworkBehaviour
{
    private void Update()
    {
        if (Object.HasInputAuthority)
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
            return;
        }

        enabled = false;
    }

}
