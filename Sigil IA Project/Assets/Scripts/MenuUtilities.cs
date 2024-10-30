using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MenuUtilities : MonoBehaviour
{
    public AudioMixer audioMixer;
    public List<Sprite> soundSwitches;

    public virtual void SwitchVolume(UnityEngine.UI.Button thisButton)
    {
        audioMixer.GetFloat("MasterVolume", out float volume);
        bool muted = (volume <= -79f);
        audioMixer.SetFloat("MasterVolume", muted ? 0f : -80f);
        thisButton.image.sprite = soundSwitches[muted ? 0 : 1];
    }

    public virtual void LoadLevelByName(string sceneName)
    {
        LoadLevel.LoadSceneByName(sceneName);
    }
}
