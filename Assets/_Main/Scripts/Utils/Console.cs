using UIBinders;

namespace Utils
{
    public class Console
    {
        public string ID { get; private set; }
        public Reactive<string> Command { get; } = new();
        
        public void Set() => ID = G.Resolve<UI>().Add(nameof(GameUIConsole), info: null);

        public void AddChar(string c) { Command.Value += c; }

        public void RemoveLastChar()
        { if (Command.Value.Length > 0) { Command.Value = Command.Value[..^1]; } }
        
        public void ForgetCommand() => Command.Value = string.Empty;
        
        public void EnterCommand()
        {
            
        }
    }
}