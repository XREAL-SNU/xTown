using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomControls : MonoBehaviour
{
    [SerializeField]
    private Button _showButton;


    // UI references
    public void OnClick_ShowRoomControls()
    {
        RoomsCanvases.Instance.CurrentRoomCanvas.Show();
    }


}
