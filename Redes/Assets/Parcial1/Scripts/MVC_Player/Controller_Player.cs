using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Player
{
    //Variables guardadas localmente
    PlayerBehaviour _playerScript;
    Model_Player _model;

    bool _moving = false;
    bool _pounding = false;

    //Constructor para pedir variables
    public Controller_Player(PlayerBehaviour playerScript, Model_Player model)
    {
        //Asignado de variables solicitadas a las locales
        _playerScript = playerScript;
        _model = model;
    }

    #region Fakes
    public void FakeStart()
    {

    }

    public void FakeUpdate()
    {
        _playerScript.InputDirX = Input.GetAxisRaw("Horizontal");
        _playerScript.InputDirY = Input.GetAxisRaw("Vertical");

        if (_playerScript.InputDirX != 0)
        {
            _moving = true;
        }
        else
        {
            _moving = false;
            _model.Still();
        }

        if (_playerScript.InputDirY < 0)
        {
            _pounding = true;
        }
        else
        {
            _pounding = false;
        }


        if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.S))
        {
            _model.Jump();
        }
    }

    public void FakeFixedUpdate()
    {
        if (_moving)
        {
            _model.Movement();
        }
        if (_pounding)
        {
            _model.Pound();
        }
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

    public void FakeOnDestroy()
    {

    }
    #endregion
}
