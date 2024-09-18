using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public static class LoadLevel
{
    public static void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public static void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
    public static void LoadSceneByNameAsync(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
    public static void LoadSceneByIndexAsync(int index)
    {
        SceneManager.LoadSceneAsync(index);
    }
    public static Scene GetActiveScene()
    {
        return SceneManager.GetActiveScene();
    }
}
