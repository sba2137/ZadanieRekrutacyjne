using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private EntityStats _playerStats;

    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = new PlayerController(GetComponent<Rigidbody2D>(), _playerStats);
    }

    private void FixedUpdate()
    {
        _playerController.HandleMovement();
    }

    private void Update()
    {
        _playerController.HandleMovementActions();
    }
}
