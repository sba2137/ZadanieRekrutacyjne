using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int GoldAmount { get; private set; }

    public EntityStats PlayerStats;

    [SerializeField] private Transform _attackPoint;

    private Rigidbody2D _rb2d;

    private Animator _animator;

    private PlayerController _playerController;

    private bool _isAlive = true;

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();

        _animator = GetComponent<Animator>();

        _playerController = new PlayerController(_rb2d, _animator, PlayerStats, gameObject.transform, _attackPoint);
    }

    private void FixedUpdate()
    {
        if (_isAlive)
            _playerController.HandleMovement();
    }

    private void Update()
    {
        if (_isAlive)
            _playerController.HandleController();
    }

    #region Main

    public void AddGold(int goldAmount)
    {
        GoldAmount += goldAmount;

        GameManager.Instance.UiManager.UpdateGoldUi(GoldAmount);
    }

    public void TakeDamage(int damageValue, Vector2 knockbackDirection)
    {
        PlayerStats.HealthPoints -= damageValue;

        GameManager.Instance.UiManager.DestroyUiHeart();

        StartCoroutine(HandleKnockback(knockbackDirection));

        if (PlayerStats.HealthPoints <= 0)
            HandleDeath();
    }

    private void HandleDeath()
    {
        _isAlive = false;

        _rb2d.simulated = false;

        GetComponent<Collider2D>().isTrigger = true;

        _animator.Play("Player_Death");

        Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);

        GameManager.Instance.UiManager.ShowDeathScreen();
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

        Gizmos.DrawWireSphere(_attackPoint.position, PlayerStats.MeleeAttackRadius);
    }

    #endregion
}
