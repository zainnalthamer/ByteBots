using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;
using System.Collections;

public class SceneLoaderFEEL : MonoBehaviour
{
    public MMF_Player fadePlayer;
    public float fadeDuration = 1f;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        fadePlayer.PlayFeedbacks();
        yield return new WaitForSeconds(fadeDuration);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone)
            yield return null;
    }
}
