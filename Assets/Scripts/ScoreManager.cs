public static class ScoreManager {
    public static int score { get; private set; } = 0;

    public static void IncreaseScore(){
        score += 10;
    }

    public static void IncreaseScore(int increment)
    {
        score += increment;
    }

    public static void DecreaseScore(){
        score -= 10;
    }

    public static void Clear() {
        score=0;
    }
}