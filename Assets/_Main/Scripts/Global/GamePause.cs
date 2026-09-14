public static class GamePause
{
    public static bool IsPaused { get; private set; }
    public static void PauseGame() => IsPaused = true;
    public static void ResumeGame() => IsPaused = false;
}