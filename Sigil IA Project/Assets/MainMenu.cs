using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    [SerializeField] private List<Sprite> soundSwitches;

    public void SwitchVolume(UnityEngine.UI.Button thisButton)
    {
        audioMixer.GetFloat("MasterVolume", out float volume);
        bool muted = (volume <= -79f);
        audioMixer.SetFloat("MasterVolume", muted ? 0f : -80f);
        thisButton.image.sprite = soundSwitches[muted ? 0 : 1];
    }

    public void LoadLevelByName(string sceneName)
    {
        LoadLevel.LoadSceneByName(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
