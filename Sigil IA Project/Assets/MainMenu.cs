using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private List<Sprite> soundSwitches;

    public void SwitchVolume(UnityEngine.UI.Button thisButton)
    {
        bool muted = videoPlayer.GetDirectAudioMute(0);
        videoPlayer.SetDirectAudioMute(0, !muted);
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
