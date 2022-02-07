using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioChatUserListing : MonoBehaviour
{
    [SerializeField]
    Text _playerNameText;
    
    void Start()
    {
        _playerNameText = GetComponentInChildren<Text>();
        
    }

    public string PlayerNameText
    {
        get => _playerNameText.text;
        set
        {
            _playerNameText.text = value;
        }
    }

    // 
    public void Remove()
    {
        Destroy(gameObject);
    }
}
