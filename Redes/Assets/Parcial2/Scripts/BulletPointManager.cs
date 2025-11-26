using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPointManager : NetworkBehaviour
{

    private void Start()
    {
        if (!Object.HasInputAuthority) Destroy(gameObject);
    }

    private void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
    }

}
