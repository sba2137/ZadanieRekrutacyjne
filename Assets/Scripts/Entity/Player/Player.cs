using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private EntityStats _playerStats;

    [SerializeField] private Transform _attackPoint;

    private Rigidbody2D _rb2d;

    private PlayerController _playerController;

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();

        _playerController = new PlayerController(_rb2d, GetComponent<Animator>(), GetComponent<SpriteRenderer>(), _playerStats, gameObject.transform, _attackPoint);
    }

    private void FixedUpdate()
    {
        _playerController.HandleMovement();
    }

    private void Update()
    {
        _playerController.HandleController();
    }

    #region Main

    public void TakeDamage(int damageValue, Vector2 knockbackDirection)
    {
        Debug.LogWarning(_playerStats.HealthPoints);

        _playerStats.HealthPoints -= damageValue;

        StartCoroutine(HandleKnockback(knockbackDirection));

        if (_playerStats.HealthPoints <= 0)
            HandleDeath();
    }

    private void HandleDeath()
    {

    }

    private IEnumerator HandleKnockback(Vector2 knockbackDirection)
    {
        _playerController.BlockMovement = true;

        _rb2d.AddForce(knockbackDirection * 5f, ForceMode2D.Impulse);

        _rb2d.AddForce(Vector2.up * 2.5f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        _rb2d.velocity = Vector2.zero;

        _playerController.BlockMovement = false;
    }

    #endregion

    #region Utility

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(_attackPoint.position, _playerStats.MeleeAttackRadius);
    }

    #endregion
}
