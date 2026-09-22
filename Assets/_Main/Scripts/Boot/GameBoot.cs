using System.Collections;
using Data;
using GameStates;
using Managers;
using Tools;
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
            Seed.SetMasterSeed(state.seed);
            
            var entities = state.entities;
            
            G.Resolve<GameCamera>().SetCamera();
            yield return null;
            
            G.Register(new BackgroundManager());
            G.Register(new ChunksGenerator());
            G.Register(new AsteroidsGenerator());
            G.Register(new CursorSpawner(entities));
            yield return null;
            
            G.Resolve<AsteroidsGenerator>().Launch();
            G.Resolve<ChunksGenerator>().Launch();
            G.Resolve<BackgroundManager>().Launch();
            G.Resolve<CursorSpawner>().Launch();
            yield return null;
            
            G.Resolve<Console>().Set();
            
            G.Resolve<States>().Activate();
            G.Resolve<States>().ChangeState<ExplorationState>();
        }
    }
}