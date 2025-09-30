using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : NetworkBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] PlayerBehaviour _owner;
    Vector3 _dir;
    
    void Start()
    {

    }

    
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.position += transform.up * _speed * Runner.DeltaTime;
    }

    public void SetDirection(Vector3 dir)
    {
        //Quaternion rot = Quaternion.LookRotation(Vector3.up, dir);
        var rot = dir.z - transform.position.z;

        transform.eulerAngles = new Vector3(0, 0, rot);
    }

    public void SetOwner(PlayerBehaviour owner)
    {
        _owner = owner;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerBehaviour _playerScript) == _owner)
        {
            print("owner");
        }

        if (collision != null)
        {
            //Destroy(gameObject);
        }

    }
}
