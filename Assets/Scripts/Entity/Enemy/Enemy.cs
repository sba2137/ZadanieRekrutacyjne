using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EntityStats _enemyStats;

    [SerializeField] private bool _isPatroling;

    [SerializeField] private Transform[] _patrolPoints;

    private Rigidbody2D _rb2d;

    private Vector2 _facingDirection;

    private int _actualFollowedPatrolPoint = 0;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    protected void HandleMovement()
    {
        if (_isPatroling && _patrolPoints.Length >= 2)
        {
            PatrolMovement();
        }

        HandleRotation();

    }
    private void PatrolMovement()
    {
        if (gameObject.transform.position.x != _patrolPoints[_actualFollowedPatrolPoint].position.x)
        {
            _rb2d.transform.position = Vector2.MoveTowards(gameObject.transform.position, new Vector2(_patrolPoints[_actualFollowedPatrolPoint].position.x, gameObject.transform.position.y), _enemyStats.MovementSpeed * Time.fixedDeltaTime);

            if (gameObject.transform.position.x == _patrolPoints[_actualFollowedPatrolPoint].position.x)
            {
                _actualFollowedPatrolPoint++;

                if (_actualFollowedPatrolPoint > _patrolPoints.Length - 1)
                    _actualFollowedPatrolPoint = 0;
            }
        }
    }

    public void TakeDamage(int damageValue)
    {
        Debug.LogWarning(_enemyStats.HealthPoints);

        _enemyStats.HealthPoints -= damageValue;

        _rb2d.AddForce(_facingDirection * 200);

        if (_enemyStats.HealthPoints <= 0)
            HandleDeath();
    }

    private void HandleDeath()
    {

    }

    private void HandleRotation()
    {
        if (_rb2d.velocity.x > 0)
            _facingDirection = Vector2.right;
        else if (_rb2d.velocity.x < 0)
            _facingDirection = Vector2.left;
    }
}
