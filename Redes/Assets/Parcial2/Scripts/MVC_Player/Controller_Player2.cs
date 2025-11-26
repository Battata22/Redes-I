using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Player2
{
    PlayerBehaviour2 _playerScript;

    public Controller_Player2(PlayerBehaviour2 playerScript)
    {
        _playerScript = playerScript;
    }

    #region Fakes
    public void FakeUpdate()
    {
        #region Old
        //_playerScript.InputDirX = Input.GetAxisRaw("Horizontal");
        //_playerScript.InputDirY = Input.GetAxisRaw("Vertical");

        //if (_playerScript.InputDirX != 0)
        //{
        //    _moving = true;
        //}
        //else
        //{
        //    _moving = false;
        //    _model.Still();
        //}

        //if (_playerScript.InputDirY < 0)
        //{
        //    _pounding = true;
        //}
        //else
        //{
        //    _pounding = false;
        //}

        //if (_playerScript.HasStateAuthority)
        //{
        //    if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.S))
        //    {
        //        _model.Jump();
        //    }

        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        _playerScript.InstantiateBullet();
        //    }
        //}

        //_model.FlipX(); 
        #endregion
    }

    public void FakeFixedUpdate()
    {
        if (!_playerScript.GetInput(out NetworkInputData inputs))
        {
            Still();
            return;
        }

        Movement(inputs.XAxis);

        if (inputs.Buttons.IsSet(ButtonTypes.Jump) && !inputs.Buttons.IsSet(ButtonTypes.Pound))
        {
            Jump();
        }

        if (inputs.Buttons.IsSet(ButtonTypes.Pound))
        {
            Pound();
        }

        if (inputs.Buttons.IsSet(ButtonTypes.MouseButton0))
        {
            _playerScript.InstantiateBullet(inputs.MousePosition);
        }

        FlipX(); 

    }

    public void FakeOnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            _playerScript.GroundTouched();
        }
    }

    public void FakeOnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            _playerScript.SetGroundedFalse();
        }
    }
    #endregion

    public void Movement(float inputX)
    {
        _playerScript.SetCaminandoAnim();

        _playerScript.Rb.velocity = new Vector2(inputX * _playerScript.Speed, _playerScript.Rb.velocity.y);
    }

    public void Jump()
    {
        _playerScript.SetGroundedFalse();

        _playerScript.SetSaltandoAnim();

        if (_playerScript.JumpsLeft > 0)
        {
            _playerScript.ReduceJump();
            _playerScript.Rb.velocity = new Vector2(_playerScript.Rb.velocity.x, 0);
            _playerScript.Rb.velocity += (Vector2.up * _playerScript.JumpForce);
        }
    }

    public void Still()
    {
        if (!_playerScript.Anim.Animator.GetBool("Idle") && !_playerScript.Anim.Animator.GetBool("Cayendo"))
        {
            _playerScript.SetIdleAnim();
        }
        _playerScript.Rb.velocity = new Vector2(0, _playerScript.Rb.velocity.y);
    }

    public void Pound()
    {
        if (_playerScript.IsGrounded) return;

        _playerScript.SetCayendoAnim();
        _playerScript.Rb.velocity += (-Vector2.up * _playerScript.PoundForce * _playerScript.Runner.DeltaTime);
    }

    public void FlipX()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _playerScript.SpriteRenderer.flipX = true;
        }
        else
        {
            _playerScript.SpriteRenderer.flipX = false;
        }
    }
}
