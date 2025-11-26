using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalInputs : NetworkBehaviour
{
    public static LocalInputs Instance { get; private set; }

    bool _isJumpPressed;
    bool _isPoundPressed;
    bool _isFirePressed;

    NetworkInputData _inputData;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            _inputData = new();
            Instance = this;
            return;
        }

        enabled = false;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJumpPressed = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            _isPoundPressed = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _isFirePressed = true;
        }
    }

    public NetworkInputData SetInputs()
    {
        _inputData.XAxis = Input.GetAxis("Horizontal");
        _inputData.YAxis = Input.GetAxis("Vertical");

        Vector3 mouse = Input.mousePosition;
        mouse.z = 0;
        _inputData.MousePosition = Camera.main.ScreenToWorldPoint(mouse);

        _inputData.Buttons.Set(ButtonTypes.MouseButton0, _isFirePressed);
        _isFirePressed = false;
        _inputData.Buttons.Set(ButtonTypes.Jump, _isJumpPressed);
        _isJumpPressed = false;
        _inputData.Buttons.Set(ButtonTypes.Pound, _isPoundPressed);
        _isPoundPressed = false;

        return _inputData;
    }

}
