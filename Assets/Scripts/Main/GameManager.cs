using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Player Player { get; private set; }
    public UiManager UiManager { get; private set; }

    [SerializeField] private Transform _playerSpawnPoint;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            Configure();
        }
    }

    private void Configure()
    {
        Player = LoadPrefab("Prefabs/Player").GetComponent<Player>();
        Player.transform.position = _playerSpawnPoint.position;

        UiManager = LoadPrefab("Prefabs/Ui").GetComponent<UiManager>();

        Camera.main.GetComponentInChildren<CinemachineVirtualCamera>().Follow = Player.transform;
        Camera.main.GetComponentInChildren<CinemachineVirtualCamera>().LookAt = Player.transform;
    }

    #region Utility

    private GameObject LoadPrefab(string prefabPath)
    {
        GameObject prefabToLoad = (GameObject)Resources.Load(prefabPath);

        if (prefabToLoad != null)
        {
            GameObject instantiatedPrefab = null;

            instantiatedPrefab = Instantiate(prefabToLoad);

            return instantiatedPrefab;
        }

        return null;
    }

    #endregion
}
