using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : NetworkBehaviour
{
    [SerializeField] float _lifetime;
    [SerializeField] float _speed;
    [SerializeField] PlayerBehaviour _owner;
    Vector3 _dir;

    public override void Spawned()
    {
        base.Spawned();
        StartCoroutine(SelfDestroy(_lifetime));
    }
    void Start()
    {
    }

    public override void FixedUpdateNetwork()
    {
        Movement();
    }

    IEnumerator SelfDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Runner.Despawn(Object);
    }

    void Movement()
    {
        transform.position += transform.up * _speed * Runner.DeltaTime;        
    }

    public void SetDirection(Vector3 dir)
    {
        ////Quaternion rot = Quaternion.LookRotation(Vector3.up, dir);
        //var rot = dir.z - transform.position.z;

        //transform.eulerAngles = new Vector3(0, 0, rot);

        var newDir = dir - transform.position;

        newDir.z = 0;

        transform.up = newDir;
    }

    public void SetOwner(PlayerBehaviour owner)
    {
        _owner = owner;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerBehaviour _playerScript) == _owner)
        {
            //print("owner");
        }
        else
        {
            Runner.Despawn(Object);
        }

    }
}
