using UnityEngine;

public class OtherScreenMenu : MonoBehaviour
{
    public void LoadLevelByName(string sceneName)
    {
        LoadLevel.LoadSceneByName(sceneName);
    }
}
