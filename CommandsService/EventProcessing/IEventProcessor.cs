namespace CommandsService.EventProcessing
{
    public interface IEventProcessor
    {
        void ProccessEvent(string message);
    }
}