namespace Ngb.Bot
{
    public interface IUpdateMessagesProcessor
    {
        void ProcessUpdateRequestAsync(string updateStrging);
    }
}
