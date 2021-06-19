using UnityEngine;

public class PlayerController
{
    private Rigidbody2D _rb2d;

    private EntityStats _playerStats;

    public PlayerController(Rigidbody2D rb2d, EntityStats playerStats)
    {
        _rb2d = rb2d;
        _playerStats = playerStats;
    }

    #region Main

    public void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        _rb2d.transform.Translate(new Vector2(horizontalInput * _playerStats.MovementSpeed * Time.fixedDeltaTime, 0));
    }

    public void HandleMovementActions()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CheckIfIsGrounded())
            Jump();
        if (Input.GetKeyUp(KeyCode.Space))
            CancelJump();
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

    #region Utility

    private bool CheckIfIsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(_rb2d.position.x - 0.1f, _rb2d.position.y), Vector2.down, 0.65f, LayerMask.GetMask("Ground"));
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(_rb2d.position.x + 0.1f, _rb2d.position.y), Vector2.down, 0.65f, LayerMask.GetMask("Ground"));

        return hit || hit2;
    }

    #endregion
}
