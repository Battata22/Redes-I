using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public float XAxis;
    public float YAxis;
    public Vector3 MousePosition;
    public const byte MOUSEBUTTON0 = 1;
    public const byte JUMP = 2;
    public const byte POUND = 3;

    public NetworkButtons Buttons;

}

public enum ButtonTypes
{
    MouseButton0 = 1,
    Jump = 2,
    Pound = 3
}
