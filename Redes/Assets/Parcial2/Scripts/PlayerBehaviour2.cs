using Fusion;
using System;
using System.Collections;
using UnityEngine;


public class PlayerBehaviour2 : NetworkBehaviour, IPlayerJoined
{
    //------------------------MVC-------------------------
    Controller_Player2 _controller;

    //------------------------Life-------------------------
    [SerializeField] int _hp;
    [Networked, OnChangedRender(nameof(LifeUpdated))] int Hp { get; set; }

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

    [SerializeField] bool _isFalling = false;
    public bool IsFalling { get { return _isFalling; } private set { _isFalling = value; } }

    [SerializeField] float _poundForce;
    public float PoundForce { get { return _poundForce; } private set { _poundForce = value; } }


    //------------------------MyData-------------------------
    [SerializeField] SpriteRenderer _spriteRenderer;
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } private set { _spriteRenderer = value; } }

    [SerializeField] NetworkMecanimAnimator _anim;
    public NetworkMecanimAnimator Anim { get { return _anim; } private set { _anim = value; } }


    //------------------------Gameplay-------------------------
    [SerializeField] PlayerTeam _team;
    public PlayerTeam Team { get { return _team; } private set { _team = value; } }

    [SerializeField] BulletBehaviour2 _bulletPrefab;
    public BulletBehaviour2 BulletPrefab { get { return _bulletPrefab; } private set { _bulletPrefab = value; } }

    [SerializeField] bool _hasJustBeenTp = false;
    public bool HasJustBeenTp { get { return _hasJustBeenTp; } private set { _hasJustBeenTp = value; } }

    [SerializeField] float _requiredPoundVelocity;
    public float RequiredPoundVelocity { get { return _requiredPoundVelocity; } private set { _requiredPoundVelocity = value; } }

    [SerializeField] float _poundDamage;
    public float PoundDamage { get { return _poundDamage; } private set { _poundDamage = value; } }

    float _lastVelocityY;

    bool _canPlay = false;

    public event Action OnDespawn;

    BulletPointManager _bulletSpawnPosition;


    public override void Spawned()
    {
        LifeBarManager2.Instance.CreateNewBar(this);

        Anim = GetComponent<NetworkMecanimAnimator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Rb = GetComponent<Rigidbody2D>();

        _controller = new(this);

        SetMaxVariable();

        GameManager2.Instance.AddToUsersList(this);

        GameManager2.Instance.OnGameEnded += PlayerCanNotStart;

        _bulletSpawnPosition = GetComponentInChildren<BulletPointManager>();
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.SessionInfo.PlayerCount >= GameManager2.Instance.MinPlayerRequiredToStart)
        {
            _canPlay = true;
        }
    }

    float waitEscape;
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            waitEscape += Time.deltaTime;
            if (waitEscape >= 1)
            {
                Application.Quit();
            }
        }
        else
        {
            waitEscape = 0;
        }

        _hp = Hp;

        if (_canPlay)
        {
            _controller.FakeUpdate();
        }

        Anim.Animator.SetBool("Grounded", _isGrounded);
        

        _lastVelocityY = Mathf.Abs(_rb.velocity.y);

        if (_rb.velocity.y < 0 && !Anim.Animator.GetBool("Cayendo") && !IsGrounded)
        {
            SetAllAnimFalse();
            SetCayendoAnim();
        }

    }

    public override void FixedUpdateNetwork()
    {

        if (_canPlay)
        {
            _controller.FakeFixedUpdate();
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
        Anim.Animator.SetBool("Cayendo", false);
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
        GameManager2.Instance.PlayerDeath(this);
        Runner.Despawn(Object);
    }


    void PlayerCanNotStart()
    {
        _canPlay = false;
        _rb.velocity = Vector2.zero;
    }

    public void InstantiateBullet(Vector3 DirBullet)
    {
        SetDisparoAnim();

        var bullet = Runner.Spawn(_bulletPrefab, transform.position, Quaternion.identity);
        //var cursorLocation = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        //var cursorLocation = Camera.main.ScreenToWorldPoint(DirBullet);
        var cursorLocation = _bulletSpawnPosition.transform.position;
        bullet.SetDirection(cursorLocation);
        bullet.SetOwner(this);

        StartCoroutine(DestroyBullet(3, bullet));
    }

    IEnumerator DestroyBullet(float time, BulletBehaviour2 bullet)
    {
        yield return new WaitForSeconds(time);
        Runner.Despawn(bullet.Object);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerBehaviour2 enemy))
        {

            GroundTouched();

            if (_lastVelocityY >= RequiredPoundVelocity)
            {
                enemy.RPC_GetDamage(PoundDamage);
            }
        }
    }

    void LifeUpdated()
    {
        OnLifeUpdate?.Invoke(Hp / (float)_maxHp);
    }

    #region Animator
    public void SetAllAnimFalse()
    {
        Anim.Animator.SetBool("Idle", false);
        Anim.Animator.SetBool("Saltando", false);
        Anim.Animator.SetBool("Cayendo", false);
        Anim.Animator.SetBool("Caminando", false);
        Anim.Animator.SetBool("Grounded", false);
    }

    public void SetIdleAnim()
    {
        SetAllAnimFalse();
        Anim.Animator.SetBool("Idle", true);
    }
    public void SetSaltandoAnim()
    {
        SetAllAnimFalse();
        Anim.Animator.SetBool("Saltando", true);
    }
    public void SetCayendoAnim()
    {
        SetAllAnimFalse();
        Anim.Animator.SetBool("Cayendo", true);
    }
    public void SetCaminandoAnim()
    {
        SetAllAnimFalse();
        Anim.Animator.SetBool("Caminando", true);
    }
    public void SetDisparoAnim()
    {
        SetAllAnimFalse();
        Anim.SetTrigger("Disparo");
    }
    public void SetAplastadoAnim()
    {
        SetAllAnimFalse();
        Anim.SetTrigger("Aplastado");
    }
    #endregion

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);

        GameManager2.Instance.OnGameEnded -= PlayerCanNotStart;
        OnDespawn?.Invoke();
    }


}
