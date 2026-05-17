namespace MusicPlayer.Strategies;

public static class StrategyCatalog
{
    public static IReadOnlyList<IPlaybackStrategy> Templates { get; } = new IPlaybackStrategy[]
    {
        new SequentialStrategy(),
        new ShuffleStrategy(),
        new SmartShuffleStrategy(),
        new RepeatOneStrategy()
    };

    public static IPlaybackStrategy CreateDefault() => new SmartShuffleStrategy();

    public static IPlaybackStrategy CreateNew(IPlaybackStrategy template) =>
        template switch
        {
            SequentialStrategy => new SequentialStrategy(),
            ShuffleStrategy => new ShuffleStrategy(),
            SmartShuffleStrategy => new SmartShuffleStrategy(),
            RepeatOneStrategy => new RepeatOneStrategy(),
            _ => new SequentialStrategy()
        };

    public static bool SameKind(IPlaybackStrategy left, IPlaybackStrategy right) =>
        left.GetType() == right.GetType();
}
