using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileDisplayPanel : MonoBehaviour
{
    [SerializeField]
    private PlayerInfoCanvas _playerInfoCanvas;
    [SerializeField]
    private PlayerInfoSection _playerInfoSection;
    [SerializeField]
    private ScheduleSection _scheduleSection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Update_PlayerInfo(string[] playerInfoData)
    {
        _playerInfoSection.Initialize(playerInfoData);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
