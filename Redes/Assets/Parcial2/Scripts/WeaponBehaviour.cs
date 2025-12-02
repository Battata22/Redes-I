using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : NetworkBehaviour
{

    [SerializeField] NetworkPrefabRef _bulletPrefab;

    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _fireSound;

    public void ShootBullet(PlayerBehaviour2 _player, Vector3 dir)
    {
        if (!HasStateAuthority) return;

        var bullet = Runner.Spawn(_bulletPrefab, transform.position, Quaternion.identity);
        var bulletScript = bullet.GetComponent<BulletBehaviour2>();
        //var cursorLocation = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        //var cursorLocation = _bulletSpawnPosition.transform.position;
        bulletScript.SetDirection(dir);
        bulletScript.SetOwner(_player);

        RPC_PlayShootSound();

        StartCoroutine(DestroyBullet(3, bulletScript));
    }

    IEnumerator DestroyBullet(float time, BulletBehaviour2 bullet)
    {
        yield return new WaitForSeconds(time);
        Runner.Despawn(bullet.Object);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RPC_PlayShootSound()
    {
        _audioSource.PlayOneShot(_fireSound);
    }

}
