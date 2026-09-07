using System.Collections;
using System.IO;
using UnityEngine;

namespace Data
{
    public class DataProvider
    {
        private static string Path => Application.persistentDataPath + "/Save.json";
        
        public GameData Data { get; private set; }
        
        public IEnumerator LoadData()
        {
            Data = null;

            if (!File.Exists(Path)) { Data = CreateData(); yield break; }
            
            var task = File.ReadAllTextAsync(Path);
            yield return new WaitUntil(() => task.IsCompleted);
            
            Data = JsonUtility.FromJson<GameData>(task.Result);
        }

        public IEnumerator SaveData()
        {
            Data ??= CreateData();
            var json = JsonUtility.ToJson(Data);
            var task = File.WriteAllTextAsync(Path, json);
            yield return new WaitUntil(() => task.IsCompleted);
        }

        private static GameData CreateData()
        {
            var data = new GameData();
            data.vSync = Configs.GameConfig.VSYNC;
            data.fps = Configs.GameConfig.FPS;
            
            return data;
        }
    }
}