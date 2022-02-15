using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Sound
{
    public string name;  //곡 이름
    public AudioClip clip;  //곡
}

public class SoundManager : MonoBehaviour
{
    #region singleton

    static public SoundManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
       
    }

    #endregion singleton
    public GameObject Player;
    public Sound[] EffectSounds;  //효과음
    public Sound[] BgmSounds;  //BGM 

    public AudioSource AudioSourceBFM;    //BGM 재생기. 
    public AudioSource[] AudioSourceEffects; //효과음 재생기. 

    public string[] PlaySoundName;  //재생 중인 효과음 사운드 이름 배열

    void Start()
    {
        PlaySoundName = new string[AudioSourceEffects.Length];
    }
    void OnEnable()
    {
    	// SceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MainRoom")
        {
            if(PlayerManager.Players.LocalPlayerGo == null){
                return;
            }
            
            Player = PlayerManager.Players.LocalPlayerGo;
            AudioSourceEffects[0] = Player.GetComponents<AudioSource>()[0];
            AudioSourceEffects[1] = Player.GetComponents<AudioSource>()[1];
            PlaySE("SpawnSound");
        }
    }

    public void PlaySE(string _name)
    {
        for(int i = 0; i< EffectSounds.Length; i++)
        {
            if(_name == EffectSounds[i].name)
            {
                for (int j = 0; j < AudioSourceEffects.Length; j++)
                {
                    if(!AudioSourceEffects[j].isPlaying)
                    {
                        AudioSourceEffects[j].clip = EffectSounds[i].clip;
                        AudioSourceEffects[j].Play();
                        PlaySoundName[j] = EffectSounds[i].name;
                        return;
                    }
                }
                Debug.Log("Using All AudioSources ");
                return;
            }
        }
        Debug.Log(_name + "Sound SoundManager에 등록되지 않음");
    }

    public void PlayBGM(string _name)
    {
        for (int i = 0; i < BgmSounds.Length; i++)
        {
            if (_name == BgmSounds[i].name)
            {
                AudioSourceBFM.clip = BgmSounds[i].clip;
                AudioSourceBFM.Play();
                return;
            }
        }
        Debug.Log(_name + "SoundManager에 등록되지 않음");
    }

    public void StopBGM()
    {
        AudioSourceBFM.Stop();
    }

    public void StopAllSE()
    {
        for (int i = 0; i < AudioSourceEffects.Length; i++)
        {
            AudioSourceEffects[i].Stop();
        }
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < AudioSourceEffects.Length; i++)
        {
            if(PlaySoundName[i] == _name)
            {
                AudioSourceEffects[i].Stop();
                break;
            }
        }
        Debug.Log("재생 중인" + _name + "사운드가 없음");
    }


}

