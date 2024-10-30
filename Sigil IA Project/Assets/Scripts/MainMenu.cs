using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;

public class MainMenu : MenuUtilities
{
    public override void SwitchVolume(UnityEngine.UI.Button thisButton)
    {
        base.SwitchVolume(thisButton);
    }

    public override void LoadLevelByName(string sceneName)
    {
        base.LoadLevelByName(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
