using System.Collections;
using Data;
using Generators;
using GameStates;
using Utils;
using Workflow.Debug;

namespace Boot
{
    public class GameBoot
    {
        public IEnumerator LaunchGame()
        {
            GameDebugService.Init();
            
            var state = G.Resolve<DataProvider>().Data;
            var entities = state.entities;
            
            G.Resolve<GameCamera>().SetCamera();
            yield return null;
            
            G.Register(new StarfieldGenerator(state.seed));
            G.Register(new AsteroidsGenerator(state.seed));
            G.Register(new CursorSpawner(entities));
            yield return null;
            
            G.Resolve<StarfieldGenerator>().Launch();
            yield return null;
            
            G.Resolve<AsteroidsGenerator>().Launch();
            yield return null;
            
            G.Resolve<CursorSpawner>().Launch();
            yield return null;
            
            G.Resolve<States>().ChangeState<ExplorationState>();
        }
    }
}