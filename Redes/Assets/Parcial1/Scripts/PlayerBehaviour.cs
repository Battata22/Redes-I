using Fusion;
using System;
using System.Collections;
using UnityEngine;


public class PlayerBehaviour : NetworkBehaviour, IPlayerJoined
{
    //------------------------MVC-------------------------
    Model_Player _model;
    View_Player _view;
    Controller_Player _controller;

    //------------------------Life-------------------------
    [SerializeField] int _hp;
    [Networked, OnChangedRender(nameof(LifeUpdated))] int Hp { get { return _hp; } /*private*/ set { _hp = value; } }

    public event Action<float> OnLifeUpdate;

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
    [SerializeField] SpriteRenderer _spriteRenderer;
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } private set { _spriteRenderer = value; } }

    //------------------------Gameplay-------------------------
    [SerializeField] PlayerTeam _team;
    public PlayerTeam Team { get { return _team; } private set { _team = value; } }

    [SerializeField] BulletBehaviour _bulletPrefab;
    public BulletBehaviour BulletPrefab { get { return _bulletPrefab; } private set { _bulletPrefab = value; } }

    [SerializeField] bool _hasJustBeenTp = false;
    public bool HasJustBeenTp { get { return _hasJustBeenTp; } private set { _hasJustBeenTp = value; } }

    [SerializeField] float _requiredPoundVelocity;
    public float RequiredPoundVelocity { get { return _requiredPoundVelocity; } private set { _requiredPoundVelocity = value; } }

    [SerializeField] float _poundDamage;
    public float PoundDamage { get { return _poundDamage; } private set { _poundDamage = value; } }

    float _lastVelocityY;

    bool _canPlay = false;

    public event Action OnDespawn;




    //private void Awake()
    //{
    //    SpriteRenderer = GetComponent<SpriteRenderer>();
    //    Rb = GetComponent<Rigidbody2D>();

    //    _model = new(this);
    //    _controller = new(this, _model);
    //    _view = new();

    //    SetMaxVariable();
    //}

    public override void Spawned()
    {
        LifeBarManager.Instance.CreateNewBar(this);

        SpriteRenderer = GetComponent<SpriteRenderer>();
        Rb = GetComponent<Rigidbody2D>();

        _model = new(this);
        _controller = new(this, _model);
        _view = new();

        SetMaxVariable();

        GameManager.Instance.AddToUsersList(this);
        //GameManager.Instance.OnGameStart += PlayerCanStart;
        GameManager.Instance.OnGameEnded += PlayerCanNotStart;
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.SessionInfo.PlayerCount >= GameManager.Instance.MinPlayerRequiredToStart)
        {
            _canPlay = true;
        }
    }

    private void Update()
    {

        if (_canPlay)
        {
            _model.FakeUpdate();
            _controller.FakeUpdate();
            _view.FakeUpdate();
        }

    }

    public override void FixedUpdateNetwork()
    {

        if (_canPlay)
        {
            _model.FakeFixedUpdate();
            _controller.FakeFixedUpdate();
            _view.FakeFixedUpdate();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _controller.FakeOnTriggerEnter2D(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _controller.FakeOnTriggerExit2D(collision);
    }

    public void ApplyTeam(PlayerTeam team, Material mat)
    {
        Team = team;
        SpriteRenderer.material = mat;

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

    public void TPed()
    {
        _spriteRenderer.enabled = false;
        HasJustBeenTp = true;
    }

    public void TPedOff()
    {
        HasJustBeenTp = false;
        _spriteRenderer.enabled = true;
    }

    // Source = quien llama al metodo  |  Target = Quien ejecuta el contenido
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_GetDamage(float dmg)
    {
        Hp -= (int)dmg;
        if (Hp <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        GameManager.Instance.PlayerDeath(this);
        Runner.Despawn(Object);
    }

    //void PlayerCanStart()
    //{
    //    _canPlay = true;
    //}
    void PlayerCanNotStart()
    {
        _canPlay = false;
        _rb.velocity = Vector2.zero;
    }

    public void InstantiateBullet()
    {
        var bullet = Runner.Spawn(_bulletPrefab, transform.position, Quaternion.identity);
        var cursorLocation = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        bullet.SetDirection(cursorLocation);
        bullet.SetOwner(this);
        //Runner.Despawn(bullet.Object);
        StartCoroutine(DestroyBullet(3, bullet));
    }

    IEnumerator DestroyBullet(float time, BulletBehaviour bullet)
    {
        yield return new WaitForSeconds(time);
        Runner.Despawn(bullet.Object);
    }

    //[SerializeField] float f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerBehaviour enemy))
        {
            if (_lastVelocityY >= RequiredPoundVelocity)
            {
                enemy.RPC_GetDamage(PoundDamage);
                //_rb.AddForce(transform.up * f);
            }
            GroundTouched();
        }
    }

    void LifeUpdated()
    {
        OnLifeUpdate?.Invoke(Hp / (float)_maxHp);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);
        //GameManager.Instance.OnGameStart -= PlayerCanStart;
        GameManager.Instance.OnGameEnded -= PlayerCanNotStart;
        OnDespawn?.Invoke();
    }


}
