using UnityEngine;

public class PlayerController
{
    private Rigidbody2D _rb2d;

    private Animator _animator;

    private SpriteRenderer _spriteRenderer;

    private EntityStats _playerStats;

    private Transform _attackPoint;

    private float _horizontalInput;

    public PlayerController(Rigidbody2D rb2d, Animator animator, SpriteRenderer spriteRenderer, EntityStats playerStats, Transform attackPoint)
    {
        _rb2d = rb2d;
        _animator = animator;
        _playerStats = playerStats;
        _attackPoint = attackPoint;
        _spriteRenderer = spriteRenderer;
    }

    #region Main

    public void HandleMovement()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");

        _rb2d.transform.Translate(new Vector2(_horizontalInput * _playerStats.MovementSpeed * Time.fixedDeltaTime, 0));

        _animator.SetFloat("HorizontalInput", Mathf.Abs(_horizontalInput));

        HandleRotation();
    }

    public void HandleActions()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CheckIfIsGrounded())
            Jump();
        if (Input.GetKeyUp(KeyCode.Space))
            CancelJump();
        if (Input.GetKeyDown(KeyCode.LeftControl))
            Attack();
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
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(_attackPoint.position, _playerStats.MeleeAttackRadius, LayerMask.GetMask("Enemies"));

        //TODO
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
            _spriteRenderer.flipX = false;
        else if (_horizontalInput < 0)
        {
            _spriteRenderer.flipX = true;
        }

    }
    #endregion
}
