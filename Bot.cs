namespace DiscordBotLogging
{
    public class Bot
    {
        public class Server
        {
            private Server(ref Bot bot) => Bot = bot;
            private RestGuild Guild;
            public Bot Bot { get; }
            public bool Exists(ulong id) => Bot.Client.GetGuild(id) != null;
            public string Name
            {
                get =>  Guild.Name;
                set => Guild.ModifyAsync((x) => x.Name = value);
            }
            public static bool Exists(ref Bot bot, ulong id) => bot.Client.GetGuild(id) != null;
            public static Server Create(ref Bot bot, string name, IVoiceRegion voiceRegion, Stream? jpegStream = null) => new(ref bot)
                {
                    Guild = bot.Client.CreateGuildAsync(name, voiceRegion, jpegStream).GetAwaiter().GetResult()
                };
            public static Server Create(ref Bot bot, string name, IVoiceRegion voiceRegion, Image? jpeg = null) => Create(ref bot, name, voiceRegion, jpeg?.Stream);
        }
        internal DiscordSocketClient Client;
        public Bot(string id)
        {

        }

    }
}