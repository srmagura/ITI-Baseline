namespace Iti.Voice
{
    public interface ITwilioVoiceSettings
    {
        string Sid { get; }
        string AuthToken { get; }
        string FromPhone { get; }
        bool UseMachineDetection { get; }
        int? MachineDetectionTimeout { get; }
    }
}