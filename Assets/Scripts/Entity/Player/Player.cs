using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private EntityStats _playerStats;

    [SerializeField] private Transform _attackPoint;

    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = new PlayerController(GetComponent<Rigidbody2D>(), GetComponent<Animator>(), GetComponent<SpriteRenderer>(), _playerStats, gameObject.transform, _attackPoint);
    }

    private void FixedUpdate()
    {
        _playerController.HandleMovement();
    }

    private void Update()
    {
        _playerController.HandleController();
    }

    #region Utility

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(_attackPoint.position, _playerStats.MeleeAttackRadius);
    }

    #endregion
}
