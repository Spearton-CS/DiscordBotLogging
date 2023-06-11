namespace DiscordBotLogging
{
    internal class Program
    {
        public static Database DB { get; set; }
        public static SocketGuild? staticGuild { get; set; } = null;
        public static DiscordSocketClient? staticClient { get; set; } = null;
        private static void Main() => RunAsync().GetAwaiter().GetResult();
        public static ServiceProvider? staticServices;
        private static async Task RunAsync()
        {
            Console.Title = "DiscordBotLogging: �������";
            DB = new();
            Info($"����: {DateTime.Now.ToLongDateString()}");
            Completed("����� ���������� ������ ������� ����� �������");
            Console.ReadKey();
            using var services = ConfigureServices();
            staticServices = services;
            var client = services.GetRequiredService<DiscordSocketClient>();
            staticClient = client;
            var commands = services.GetRequiredService<InteractionService>();
            client.Log += LogAsync;
            client.ButtonExecuted += Client_ButtonExecuted;
            commands.Log += LogAsync;
            await services.GetRequiredService<CommandHandler>().InitializeAsync();
            client.Ready += async () =>
            {
                staticGuild = client.GetGuild(AppSettings.Server);
                if (staticGuild is not null)
                    Completed("������ ������");
                else
                {
                    Error("�� ������� ����� ������, ���������� ���������������!");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                await commands.RegisterCommandsToGuildAsync(staticGuild.Id, true);
                Completed("������� ��������� �/��� ��������");
            };
        Start:;
            await client.LoginAsync(TokenType.Bot, AppSettings.Token);
            await client.StartAsync();
            await client.SetGameAsync("������ ������", "https://music.yandex.ru", ActivityType.Listening);
        CommandsRead:
            Completed("�� ������ ������ �������!");
            string input = Console.ReadLine();
            switch (input.ToLower())
            {
                default:
                    Console.WriteLine($"����������� ������� {input}, ��������� �������: stop, start");
                    goto CommandsRead;
                case "stop":
                    client.LogoutAsync();
                    client.StopAsync();
                    Warning("��� ��������!");
                    goto CommandsRead;
                case "start":
                    goto Start;
            }
        }
        private static Task Client_ButtonExecuted(SocketMessageComponent arg)
        {
            return Task.CompletedTask;
        }
        private static Task LogAsync(LogMessage arg)
        {
            Info(arg);
            return Task.CompletedTask;
        }
        private static void WriteLine(ConsoleColor Color, object Text)
        {
            Console.ForegroundColor = Color;
            Console.WriteLine(Text);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Error(object Text) => WriteLine(ConsoleColor.Red, Text);
        public static void Info(object Text) => WriteLine(ConsoleColor.Blue, Text);
        public static void Completed(object Text) => WriteLine(ConsoleColor.Green, Text);
        public static void Warning(object Text) => WriteLine(ConsoleColor.Yellow, Text);
        private static ServiceProvider ConfigureServices() => new ServiceCollection().AddSingleton<DiscordSocketClient>().AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>())).AddSingleton<CommandHandler>().BuildServiceProvider();
    }
}