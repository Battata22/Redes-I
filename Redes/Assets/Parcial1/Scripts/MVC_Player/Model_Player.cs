using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Player
{
    //Variables guardadas localmente
    PlayerBehaviour _playerScript;

    //Constructor para pedir variables
    public Model_Player(PlayerBehaviour playerScript)
    {
        //Asignado de variables solicitadas a las locales
        _playerScript = playerScript;
    }

    #region Fakes
    public void FakeStart()
    {

    }

    public void FakeUpdate()
    {

    }

    public void FakeFixedUpdate()
    {

    }

    public void FakeOnDestroy()
    {

    }
    #endregion

    public void Movement()
    {
        #region Nono
        //if (Mathf.Abs(_playerScript.Rb.velocity.x) > _playerScript.Speed) return;
        //if (_playerScript.Rb.velocity.x > _playerScript.Speed || _playerScript.Rb.velocity.x < -_playerScript.Speed) return;
        //if (_playerScript.InputDir > 0 && _playerScript.Rb.velocity.x < 0) _playerScript.Rb.velocity = new Vector2(-_playerScript.Rb.velocity.x, _playerScript.Rb.velocity.y);
        //if (_playerScript.InputDir < 0 && _playerScript.Rb.velocity.x > 0) _playerScript.Rb.velocity = new Vector2(_playerScript.Rb.velocity.x, _playerScript.Rb.velocity.y);

        //_dir = new Vector2(_playerScript.InputDir, 0);

        ////_playerScript.Rb.position += _dir * _playerScript.Speed * _playerScript.Runner.DeltaTime;
        //_playerScript.Rb.velocity += _dir * _playerScript.Speed * _playerScript.Runner.DeltaTime;
        ////_playerScript.Rb.AddForce(_dir * _playerScript.Speed * _playerScript.Runner.DeltaTime, ForceMode2D.Impulse);

        //if(Mathf.Abs(_playerScript.Rb.velocity.x) > _playerScript.Speed)
        //    _playerScript.Rb.velocity = new Vector2(_playerScript.Speed * _playerScript.InputDir, _playerScript.Rb.velocity.y); 
        #endregion

        _playerScript.Rb.velocity = new Vector2(_playerScript.InputDirX * _playerScript.Speed, _playerScript.Rb.velocity.y);

    }

    public void Jump()
    {
        _playerScript.SetGroundedFalse();

        if (_playerScript.JumpsLeft > 0)
        {
            _playerScript.ReduceJump();
            _playerScript.Rb.velocity = new Vector2(_playerScript.Rb.velocity.x, 0);
            _playerScript.Rb.velocity += (Vector2.up * _playerScript.JumpForce);
        }
    }

    public void Still()
    {
        _playerScript.Rb.velocity = new Vector2(0, _playerScript.Rb.velocity.y);
    }

    public void Pound()
    {
        if (_playerScript.IsGrounded) return;
        _playerScript.Rb.velocity += (-Vector2.up * _playerScript.PoundForce * _playerScript.Runner.DeltaTime);
    }


}
