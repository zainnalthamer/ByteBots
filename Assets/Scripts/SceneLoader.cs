using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] string loadingSceneName = "MMAdditiveLoadingScreen";

    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    } 

    public void LoadScene(string sceneName)
    {
        MMSceneLoadingManager.LoadingScreenSceneName = loadingSceneName;
        MMAdditiveSceneLoadingManager.LoadScene(sceneName, loadingSceneName, ThreadPriority.High, true, true, 0, 0.3f,0,0,0.3f, null,null,5,
            MMAdditiveSceneLoadingManager.FadeModes.FadeInThenOut,
            MMAdditiveSceneLoadingManagerSettings.UnloadMethods.AllScenes);
    }

}
