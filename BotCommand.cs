namespace DiscordBotLogging
{
    public class BotCommand
    {
        public enum ApplyType
        {
            Server,
            Channel,
            DM
        }
        public ApplyType Apply { get; }
        public ulong ApplyID { get; }
        public string Action { get; }
        public BotCommand(ref Bot bot, ApplyType apply, ulong applyId, string act)
        {
            if ((Apply == ApplyType.Server))
            {

            }
            else
            {

            }
        }
    }
}