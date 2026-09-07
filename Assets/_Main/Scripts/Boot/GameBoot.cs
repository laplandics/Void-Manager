using System.Collections;
using Data;
using Managers.UIManagers;

namespace Boot
{
    public class GameBoot
    {
        public IEnumerator LaunchGame()
        {
            var state = G.Resolve<DataProvider>().Data;
            
            G.Register(new GameUIManager());
            G.Resolve<GameUIManager>().Activate();
            
            G.Register(new EntityUIManager());
            G.Resolve<EntityUIManager>().Activate();
            
            yield return null;
        }
    }
}