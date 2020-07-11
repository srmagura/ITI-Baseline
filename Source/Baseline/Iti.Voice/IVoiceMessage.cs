namespace Iti.Baseline.Voice
{
    public interface IVoiceMessage
    {
        string ContentMarkup { get; }
        void Say(string message, int? loop = null);
        void Pause(int seconds);
    }
}