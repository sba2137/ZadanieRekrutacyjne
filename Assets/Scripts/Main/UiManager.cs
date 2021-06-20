using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _goldAmountText;

    [SerializeField] private Transform _heartsTransform;

    [SerializeField] private GameObject _uiHeartPrefab;

    [SerializeField] private GameObject _deathScreen;

    private List<GameObject> _uiHearts;

    private void Start()
    {
        _uiHearts = new List<GameObject>();

        for (int i = 0; i < GameManager.Instance.Player.PlayerStats.HealthPoints; i++)
        {
            AddUiHeart();
        }
    }

    #region Main

    public void UpdateGoldUi(int goldAmount) => _goldAmountText.text = goldAmount.ToString();

    public void AddUiHeart()
    {
        GameObject spawnedHeart = Instantiate(_uiHeartPrefab, _heartsTransform);

        _uiHearts.Add(spawnedHeart);
    }

    public void DestroyUiHeart()
    {
        if (_uiHearts.Count > 0)
        {
            Animator heartAnimator = _uiHearts[_uiHearts.Count - 1].GetComponent<Animator>();

            heartAnimator.SetTrigger("HeartLost");

            Destroy(_uiHearts[_uiHearts.Count - 1], 1);

            _uiHearts.RemoveAt(_uiHearts.Count - 1);
        }
    }

    public void ShowDeathScreen() => _deathScreen.SetActive(true);

    #endregion

    #region Buttons Logic

    public void RestartButton() => SceneManager.LoadScene("Scene_0");

    #endregion
}
