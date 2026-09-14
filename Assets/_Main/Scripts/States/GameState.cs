using Constants;

namespace GameStates
{
    public abstract class GameState
    {
        public virtual void SetParameters(StateParameters _) {}
        public abstract void OnEnter();
        public abstract void OnKeyPressed(KeyboardKeys key);
        public abstract void OnExit();
    }

    public class StateParameters
    {
        public readonly object[] Parameters;

        public StateParameters(params object[] parameters)
        { Parameters = parameters; }
    }
}