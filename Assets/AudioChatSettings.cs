using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChatSettings : MonoBehaviour
{
    Recorder _recorder;
    // Start is called before the first frame update
    void Start()
    {
        _recorder = GetComponent<Recorder>();
        var devices = _recorder.MicrophonesEnumerator;
        foreach(var device in devices)
        {
            Debug.Log("device: " + device.IDString);
        }
        GetComponent<Recorder>().MicrophoneDevice = devices.GetEnumerator().Current;
    }

    
}
