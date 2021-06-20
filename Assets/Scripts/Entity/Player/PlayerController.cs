using UnityEngine;

public class PlayerController
{
    private Rigidbody2D _rb2d;

    private Animator _animator;

    private SpriteRenderer _spriteRenderer;

    private EntityStats _playerStats;

    private Transform _attackPoint;

    private Transform _playerTransform;

    private float _horizontalInput;

    private float _attackCooldown;

    public PlayerController(Rigidbody2D rb2d, Animator animator, SpriteRenderer spriteRenderer, EntityStats playerStats, Transform playerTransform, Transform attackPoint)
    {
        _rb2d = rb2d;
        _animator = animator;
        _playerStats = playerStats;
        _attackPoint = attackPoint;
        _playerTransform = playerTransform;
        _spriteRenderer = spriteRenderer;
    }

    #region Main

    public void HandleMovement()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");

        _rb2d.transform.Translate(new Vector2(_horizontalInput * _playerStats.MovementSpeed * Time.fixedDeltaTime, 0));

        HandleRotation();
    }

    public void HandleController()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CheckIfIsGrounded())
            Jump();
        if (Input.GetKeyUp(KeyCode.Space))
            CancelJump();
        if (Input.GetKeyDown(KeyCode.LeftControl) && _attackCooldown <= 0)
            Attack();

        HandleAnimations();

        HandleAttackCooldown();
    }

    private void HandleAnimations()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack"))
        {
            if (_horizontalInput != 0 && _rb2d.velocity.y == 0)
                _animator.Play("Player_Run");
            if (_rb2d.velocity.y > 0.25f)
                _animator.Play("Player_Jump");
            if (_rb2d.velocity.y < 0)
                _animator.Play("Player_Fall");
            if (_horizontalInput == 0 && _rb2d.velocity.y == 0)
                _animator.Play("Player_Idle");
        }
    }

    #endregion

    #region Movement Actions

    private void Jump()
    {
        _rb2d.AddForce(Vector2.up * _playerStats.JumpForce, ForceMode2D.Impulse);
    }

    private void CancelJump()
    {
        _rb2d.velocity = new Vector2(_rb2d.velocity.x, _rb2d.velocity.y * 0.5f);
    }

    #endregion

    #region Combat Actions

    private void Attack()
    {
        _animator.Play("Player_Attack");

        _attackCooldown = _playerStats.AttackSpeed;

        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(_attackPoint.position, _playerStats.MeleeAttackRadius, LayerMask.GetMask("Enemies"));

        foreach (var enemy in enemiesInRange)
        {
            enemy.GetComponent<Enemy>().TakeDamage(_playerStats.MeleeAttackDamage);
        }
    }

    #endregion

    #region Utility

    private bool CheckIfIsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(_rb2d.position.x - 0.1f, _rb2d.position.y), Vector2.down, 0.65f, LayerMask.GetMask("Ground"));
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(_rb2d.position.x + 0.1f, _rb2d.position.y), Vector2.down, 0.65f, LayerMask.GetMask("Ground"));

        return hit || hit2;
    }

    private void HandleRotation()
    {
        if (_horizontalInput > 0)
            _playerTransform.localScale = new Vector3(1, 1, 1);
        else if (_horizontalInput < 0)
            _playerTransform.localScale = new Vector3(-1, 1, 1);
    }

    private void HandleAttackCooldown()
    {
        if (_attackCooldown > 0)
            _attackCooldown -= Time.deltaTime;
    }
    #endregion
}
