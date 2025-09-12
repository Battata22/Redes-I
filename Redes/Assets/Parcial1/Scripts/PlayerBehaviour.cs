using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBehaviour : NetworkBehaviour
{
    //------------------------MVC-------------------------
    Model_Player _model;
    View_Player _view;
    Controller_Player _controller;

    //------------------------Life-------------------------
    [SerializeField] int _hp;
    public int Hp { get { return _hp; } private set { _hp = value; } }

    [SerializeField] int _maxHp;
    public int MaxHp { get { return _maxHp; } private set { _maxHp = value; } }

    //------------------------Movement-------------------------
    [SerializeField] float _speed;
    public float Speed { get { return _speed; } private set { _speed = value; } }

    [SerializeField] Rigidbody2D _rb;
    public Rigidbody2D Rb { get { return _rb; } private set { _rb = value; } }

    [NonSerialized] public float InputDirX, InputDirY;

    [SerializeField] float _jumpForce;
    public float JumpForce { get { return _jumpForce; } private set { _jumpForce = value; } }

    [SerializeField] int _jumpsAmount;
    public int JumpsMaxAmount { get { return _jumpsAmount; } private set { _jumpsAmount = value; } }

    [SerializeField] int _jumpsLeft;
    public int JumpsLeft { get { return _jumpsLeft; } private set { _jumpsLeft = value; } }

    [SerializeField] int _jumpCost;
    public int JumpCost { get { return _jumpCost; } private set { _jumpCost = value; } }

    [SerializeField] bool _isGrounded = false;
    public bool IsGrounded { get { return _isGrounded; } private set { _isGrounded = value; } }

    [SerializeField] float _poundForce;
    public float PoundForce { get { return _poundForce; } private set { _poundForce = value; } }


    //------------------------MyData-------------------------
    [SerializeField] Material _matPlayer;
    public Material MatPlayer { get { return _matPlayer; } private set { _matPlayer = value; } }

    //------------------------Gameplay-------------------------
    [SerializeField] PlayerTeam _team;
    public PlayerTeam Team { get { return _team; } private set { _team = value; } }


    private void Awake()
    {
        MatPlayer = GetComponent<SpriteRenderer>().material;
        Rb = GetComponent<Rigidbody2D>();

        _model = new(this);
        _controller = new(this, _model);
        _view = new();

        SetMaxVariable();
    }

    private void Update()
    {
        _model.FakeUpdate();
        _controller.FakeUpdate();
        _view.FakeUpdate();


    }

    public override void FixedUpdateNetwork()
    {
        _model.FakeFixedUpdate();
        _controller.FakeFixedUpdate();
        _view.FakeFixedUpdate();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _controller.FakeOnTriggerEnter2D(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _controller.FakeOnTriggerExit2D(collision);
    }

    public void ApplyTeam(PlayerTeam team, Color color)
    {
        Team = team;
        MatPlayer.color = color;

    }

    public void GroundTouched()
    {
        IsGrounded = true;
        JumpsLeft = JumpsMaxAmount;
    }

    public void SetGroundedFalse()
    {
        IsGrounded = false;
    }

    void SetMaxVariable()
    {
        JumpsLeft = JumpsMaxAmount;
        Hp = MaxHp;
    }

    public void ReduceJump()
    {
        JumpsLeft -= JumpCost;
    }
}
