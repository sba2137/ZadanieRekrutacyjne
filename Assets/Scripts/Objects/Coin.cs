using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _coinValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.Player.AddGold(_coinValue);

            Destroy(gameObject);
        }
    }
}
