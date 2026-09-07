using System.Collections;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Boot
{
    public class Boot
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap() => _ = new Boot();

        private Boot()
        {
            G.Register(new UI());
            G.Register(new Scenes());
            G.Register(new Systems());
            G.Register(new Entities());
            G.Register(new Coroutines());
            G.Register(new DataProvider());
            
            //DEBUG ONLY
            Debug.LogWarning("DEBUG SCENE CHECK");
            var sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "Test")
            { Debug.Log("Test scene loaded"); return; }
            //DEBUG ONLY
            
            G.Resolve<Coroutines>().Start(LoadProject());
        }

        private IEnumerator LoadProject()
        {
            yield return Resources.UnloadUnusedAssets();
            yield return G.Resolve<Scenes>().ToBoot();
            yield return null;

            yield return G.Resolve<DataProvider>().LoadData();
            var data = G.Resolve<DataProvider>().Data;
            if (data.vSync > 0)
            {
                QualitySettings.vSyncCount = data.vSync;
                Application.targetFrameRate = -1;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = data.fps;
            }
            
            G.Resolve<Systems>().Initialize();
            yield return null;
            
            G.Resolve<Coroutines>().Start(LoadGame());
        }

        private IEnumerator LoadGame()
        {
            yield return G.Resolve<Scenes>().ToScene("Game");
            yield return G.Resolve<UI>().SetUI();
            
            var game = new GameBoot();
            yield return game.LaunchGame();

            yield return Resources.UnloadUnusedAssets();
            yield return null;
        }
    }
}
