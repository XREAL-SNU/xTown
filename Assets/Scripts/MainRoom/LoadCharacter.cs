using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using StarterAssets;

public class LoadCharacter : MonoBehaviour
{
    public GameObject[] CharacterPrefabs;
    public Transform SpawnPoint;
    public CinemachineFreeLook FreeLookCam;
    private Transform FollowTarget;
    private GameObject Player;
    public ThirdPersonControllerMulti PlayerControl
    {
        get => Player.GetComponent<ThirdPersonControllerMulti>();
        private set
        {
            return;
        }
    }
    private GameObject _prefab;
    public static LoadCharacter Instance = null;

    // singleton
    private void Awake()
    {
        // singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// Monobeviour callbacks
    /// </summary>
    void OnEnable()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        _prefab = CharacterPrefabs[selectedCharacter];
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        Debug.Log("Load Character disabled");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void Update()
    {
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player");
            if (Player != null)
            {
                //FollowTarget = Player.transform;
                FollowTarget = GameObject.Find("FollowTarget").transform;
                FreeLookCam.Follow = Player.transform;
                FreeLookCam.LookAt = FollowTarget;
            }
        }
    }

    /// <summary>
    /// Scene callbacks.
    /// </summary>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("LoadCharacter/OnSceneLoaded: " + scene.name);
        SpawnPoint = GameObject.Find("SpawnPoint").transform;
        FreeLookCam = GameObject.Find("CharacterCam").GetComponent<CinemachineFreeLook>();

        // disable currentRoomCanvas
        RoomsCanvases.Instance.CurrentRoomCanvas.Hide();
        InitCharacter();
    }

    /// <summary>
    /// Private members
    /// </summary>

    void InitCharacter()
    {
        if(!PhotonNetwork.InRoom || !PhotonNetwork.IsConnected)
        {// instantiate locally
            Debug.Log("LoadCharacter/Instantiating player locally");
            Player = Instantiate(_prefab, SpawnPoint.position, Quaternion.identity);

        }
        else
        {
            // instantiate over the network
            Debug.Log("LoadCharacter/Instantiating player over the network");
            Player = PhotonNetwork.Instantiate("CharacterPrefab", SpawnPoint.position, Quaternion.identity);

        }
        PlayerManager.Players.LocalPlayerGo = Player;

        //bind camera
        FollowTarget = Player.transform.Find("FollowTarget");
        FreeLookCam.Follow = Player.transform;
        FreeLookCam.LookAt = FollowTarget;
    }

}
