using Jurd.Utilities;
using System.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jurd.Networking.ConnectToServer
{
    public enum SceneType
    {
        None = 0x0,
        Lobby = 0x1, // hmmm
        Main = 0x2,
        MainMenu = 0x4,
    }

    public struct SceneInfo : IComponentData
    {
        public SceneType SceneType;
        public Unity.Entities.Hash128 SceneGuid;
    }

    public class SceneLoader : Singleton<SceneLoader>
    {
        public void LoadScene(SceneType sceneType)
        {
            StartCoroutine(LoadSceneAsync(sceneType.ToString()));
        }

        protected IEnumerator LoadSceneAsync(string sceneName)
        {
            //LoadingScreen.Instance.ShowLoadingScreen(true);

            var operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                if (operation.progress >= 0.9f)
                {
                    operation.allowSceneActivation = true;
                }
                yield return null;
            }

            //LoadingScreen.Instance.ShowLoadingScreen(false);
        }
    }
}
