using Fusion;
using UnityEngine;

public class BulletBehaviour : NetworkBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _damage;
    [SerializeField] PlayerBehaviour _owner;
    [SerializeField] Rigidbody2D _rb;

    public override void Spawned()
    {
        base.Spawned();

        //_rb = GetComponent<Rigidbody2D>();
    }

    public override void FixedUpdateNetwork()
    {
        Movement();
    }

    void Movement()
    {
        var dir = transform.up * _speed * Runner.DeltaTime;
        var newdir = new Vector2(dir.x, dir.y);
        _rb.position += newdir;

        //transform.position += transform.up * _speed * Runner.DeltaTime;        
    }

    public void SetDirection(Vector3 dir)
    {
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
        if (!HasStateAuthority) return;

        if (collision.GetComponent<PlayerBehaviour>())
        {
            var hit = collision.GetComponent<PlayerBehaviour>();

            if (hit == _owner)
            {
                //print("owner");
            }
            else
            {
                Runner.Despawn(Object);
                hit.RPC_GetDamage(_damage);
                print("hit player");
            }
        }
        else
        {
            //print("choco con " +  collision.name);
            Runner.Despawn(Object);
        }

    }
}
