using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour {
    
    public static GameManager instance;
    
    [Header("Player")]
    [SerializeField] private PlayerController[] playerPrefabs;
    private int _index = 0;
    private int _playerCount;
    
    [Header("UI")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject[] aliveIcons;
    [SerializeField] private GameObject[] deadIcons;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSourceObject;
    
    public override void OnNetworkSpawn() {
        SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        EnableTimeServerRpc();
        
        gameOverUI.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayerServerRpc(ulong playerId) {
        var spawn = Instantiate(playerPrefabs[_index]);
        spawn.NetworkObject.SpawnWithOwnership(playerId);
        EnableIconUIForAllClientsServerRpc(_index);
        _index++;
        _playerCount++;
    }
    
    public override void OnDestroy() {
        base.OnDestroy();
        StopAllCoroutines();
        MatchmakingService.LeaveLobby().ContinueWith(
            result => {
                if(NetworkManager.Singleton != null && (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsClient)) NetworkManager.Singleton.Shutdown();
            });
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlaySound(AudioClip clip, Transform spawnPoint, float volume)
    {
        AudioSource audioSource = Instantiate(audioSourceObject, spawnPoint.position, Quaternion.identity);
        
        audioSource.clip = clip;
        
        audioSource.volume = volume;
        
        audioSource.Play();
        
        float clipLength = clip.length;
        
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayerDie(int playerIndex)
    {
        _playerCount--;

        PlayerDeathForAllClientsServerRpc(playerIndex);
        
        if (_playerCount <= 1)
        {
            GameOverForAllClientsServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void GameOverForAllClientsServerRpc()
    {
        GameOverForAllClientsClientRpc();
    }

    [ClientRpc]
    private void GameOverForAllClientsClientRpc()
    {
        GameOver();
    }
    
    private void GameOver()
    {
        gameOverUI.SetActive(true);
    }

    [ServerRpc(RequireOwnership = false)]
    private void EnableIconUIForAllClientsServerRpc(int index)
    {
        EnableIconUIForAllClientsClientRpc(index);
    }
    
    [ClientRpc]
    private void EnableIconUIForAllClientsClientRpc(int index)
    {
        EnableIconUI(index);
    }

    private void EnableIconUI(int index)
    {
        aliveIcons[index].SetActive(true);
        deadIcons[index].SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    private void PlayerDeathForAllClientsServerRpc(int playerIndex)
    {
        PlayerDeathForAllClientsClientRpc(playerIndex);
    }
    
    [ClientRpc]
    private void PlayerDeathForAllClientsClientRpc(int playerIndex)
    {
        PlayerDeathUI(playerIndex);
    }
    
    private void PlayerDeathUI(int playerIndex)
    {
        aliveIcons[playerIndex].SetActive(false);
        deadIcons[playerIndex].SetActive(true);
    }

    [ServerRpc(RequireOwnership = false)]
    private void EnableTimeServerRpc()
    {
        Time.timeScale = 1;
    }
}