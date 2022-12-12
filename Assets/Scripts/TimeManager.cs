public static class TimeManager {

    public static int timeRemaining { get; set;} = 60;

    public static void Clear()
    {
        timeRemaining = 60;
    }
}