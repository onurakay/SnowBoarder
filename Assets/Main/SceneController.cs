using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // singleton
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        // only one instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ReloadScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        Debug.Log($"Reloading Scene: {activeScene.name}");
        SceneManager.LoadScene(activeScene.buildIndex);
    }

    public void LoadScene(int sceneIndex)
    {
        Debug.Log($"Loading Scene Index: {sceneIndex}");
        SceneManager.LoadScene(sceneIndex);
    }
}
