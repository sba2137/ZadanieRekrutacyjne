using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ForegroundHidder : MonoBehaviour
{
    [SerializeField] private float _cutoffTime;

    private Tilemap _tileMap;

    private Coroutine _alphaChange;

    private void Start() => _tileMap = GetComponent<Tilemap>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_alphaChange != null)
                StopCoroutine(_alphaChange);

            _alphaChange = StartCoroutine(ChangeTilemapAlpha(true));
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_alphaChange != null)
                StopCoroutine(_alphaChange);

            _alphaChange = StartCoroutine(ChangeTilemapAlpha(false));
        }
    }

    private IEnumerator ChangeTilemapAlpha(bool decreaseAlpha)
    {
        if (decreaseAlpha)
        {
            while (_tileMap.color.a > 0)
            {
                _tileMap.color = new Color(_tileMap.color.r, _tileMap.color.g, _tileMap.color.b, _tileMap.color.a - 0.1f);

                yield return new WaitForSeconds(_cutoffTime);
            }
        }

        else
        {
            while (_tileMap.color.a < 1)
            {
                _tileMap.color = new Color(_tileMap.color.r, _tileMap.color.g, _tileMap.color.b, _tileMap.color.a + 0.1f);

                yield return new WaitForSeconds(_cutoffTime);
            }
        }
    }
}
