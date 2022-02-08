using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        BallBounce,
        RimHit,
        Goal,
    }


    private static GameObject _oneShotGameObject;
    private static AudioSource _oneShotAudioSource;

    public static void PlaySound(Sound sound, Vector3 position, float volume)
    {
        GameObject soundGameObject = new GameObject("Sound");
        soundGameObject.transform.position = position;
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = GetAudioClip(sound);
        audioSource.volume = volume;
        audioSource.spatialBlend = 1;
        audioSource.Play();
        Object.Destroy(soundGameObject, audioSource.clip.length);
    }

    public static void PlaySound(Sound sound, float volume)
    {
        if (_oneShotGameObject == null)
        {
            _oneShotGameObject = new GameObject("One Shot Sound");
            _oneShotAudioSource= _oneShotGameObject.AddComponent<AudioSource>();
        }
        _oneShotAudioSource.PlayOneShot(GetAudioClip(sound), volume);
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (BasketballAssets.SoundAudioClip soundAudioClip in BasketballAssets.i.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }
}
