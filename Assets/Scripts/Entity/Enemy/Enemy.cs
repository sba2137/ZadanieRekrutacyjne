using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Main Settings")]

    [SerializeField] private EntityStats _enemyStats;

    [Header("Movement Settings")]

    [SerializeField] private bool _isPatroling;

    [SerializeField] private Transform[] _patrolPoints;

    private Rigidbody2D _rb2d;

    private Animator _animator;

    private SpriteRenderer _spriteRenderer;

    private Vector2 _facingDirection;

    private int _actualFollowedPatrolPoint;

    private bool _isMoving;

    private bool _isAlive = true;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void Update()
    {
        HandleAnimations();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<Player>().TakeDamage(1, -_facingDirection);
    }

    #region Movement

    protected void HandleMovement()
    {
        if (_isAlive)
        {
            if (_isPatroling && _patrolPoints.Length >= 2)
            {
                _isMoving = true;

                PatrolMovement();
            }

            else
                _isMoving = false;

            HandleRotation();
        }
    }
    private void PatrolMovement()
    {
        if (gameObject.transform.position.x != _patrolPoints[_actualFollowedPatrolPoint].position.x)
        {

            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, new Vector2(_patrolPoints[_actualFollowedPatrolPoint].position.x, gameObject.transform.position.y), _enemyStats.MovementSpeed * Time.fixedDeltaTime);

            if (gameObject.transform.position.x == _patrolPoints[_actualFollowedPatrolPoint].position.x)
            {
                _actualFollowedPatrolPoint++;

                if (_actualFollowedPatrolPoint > _patrolPoints.Length - 1)
                    _actualFollowedPatrolPoint = 0;
            }
        }
    }

    #endregion

    #region Main

    public void TakeDamage(int damageValue, Vector2 knockbackDirection)
    {
        _animator.Play("Enemy_Hurt");

        _enemyStats.HealthPoints -= damageValue;

        StartCoroutine(HandleKnockback(knockbackDirection));

        if (_enemyStats.HealthPoints <= 0)
            HandleDeath();
    }

    private void HandleDeath()
    {
        _rb2d.bodyType = RigidbodyType2D.Static;

        GetComponent<Collider2D>().isTrigger = true;

        _isAlive = false;

        _animator.Play("Enemy_Death");

        Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
    }

    private IEnumerator HandleKnockback(Vector2 knockbackDirection)
    {
        _rb2d.AddForce(knockbackDirection * 5f, ForceMode2D.Impulse);

        _rb2d.AddForce(Vector2.up * 2.5f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        _rb2d.velocity = Vector2.zero;
    }

    #endregion

    #region Utility

    private void HandleRotation()
    {
        if (_patrolPoints.Length > 0)
        {
            if (_patrolPoints[_actualFollowedPatrolPoint].transform.position.x < gameObject.transform.position.x)
            {
                _facingDirection = Vector2.left;
                _spriteRenderer.flipX = true;
            }

            else
            {
                _facingDirection = Vector2.right;
                _spriteRenderer.flipX = false;
            }
        }
    }

    private void HandleAnimations()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Hurt") && _isAlive)
        {
            if (_isMoving)
                _animator.Play("Enemy_Walk");
            else
                _animator.Play("Enemy_Idle");
        }
    }

    #endregion
}
