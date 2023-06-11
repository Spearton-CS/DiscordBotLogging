namespace DiscordBotLogging
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _interactionCommands;
        private readonly IServiceProvider _services;
        public CommandHandler(DiscordSocketClient client, InteractionService interactionCommands, IServiceProvider services)
        {
            _client = client;
            _interactionCommands = interactionCommands;
            _services = services;
        }
        public async Task InitializeAsync()
        {
            await _interactionCommands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _client.InteractionCreated += HandleInteraction;
            _interactionCommands.SlashCommandExecuted += SlashInteractionCommandExecuted;
            _interactionCommands.ContextCommandExecuted += ContextInteractionCommandExecuted;
            _interactionCommands.ComponentCommandExecuted += ComponentInteractionCommandExecuted;
        }
        public async Task DeInitialiseAsync()
        {
            _client.InteractionCreated -= HandleInteraction;
            _interactionCommands.SlashCommandExecuted -= SlashInteractionCommandExecuted;
            _interactionCommands.ContextCommandExecuted -= ContextInteractionCommandExecuted;
            _interactionCommands.ComponentCommandExecuted -= ComponentInteractionCommandExecuted;
        }
        private Task ComponentInteractionCommandExecuted(ComponentCommandInfo arg1, IInteractionContext arg2, Discord.Interactions.IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        break;
                    case InteractionCommandError.UnknownCommand:
                        break;
                    case InteractionCommandError.BadArgs:
                        break;
                    case InteractionCommandError.Exception:
                        break;
                    case InteractionCommandError.Unsuccessful:
                        break;
                    default:
                        break;
                }
            }
            return Task.CompletedTask;
        }
        private Task ContextInteractionCommandExecuted(ContextCommandInfo arg1, IInteractionContext arg2, Discord.Interactions.IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        Program.Error(arg3.ErrorReason + " [UnmetPrecondition]");
                        break;
                    case InteractionCommandError.UnknownCommand:
                        Program.Error(arg3.ErrorReason + " [UnknownCommand]");
                        break;
                    case InteractionCommandError.BadArgs:
                        Program.Error(arg3.ErrorReason + " [BadArgs]");
                        break;
                    case InteractionCommandError.Exception:
                        Program.Error(arg3.ErrorReason + " [Exception]");
                        break;
                    case InteractionCommandError.Unsuccessful:
                        Program.Error(arg3.ErrorReason + " [Unsuccefsul]");
                        break;
                    default:
                        Program.Error(arg3.ErrorReason);
                        break;
                }
            }
            return Task.CompletedTask;
        }
        private Task SlashInteractionCommandExecuted(SlashCommandInfo arg1, IInteractionContext arg2, Discord.Interactions.IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        Program.Error(arg3.ErrorReason + " [UnmetPrecondition]");
                        break;
                    case InteractionCommandError.UnknownCommand:
                        Program.Error(arg3.ErrorReason + " [UnknownCommand]");
                        break;
                    case InteractionCommandError.BadArgs:
                        Program.Error(arg3.ErrorReason + " [BadArgs]");
                        break;
                    case InteractionCommandError.Exception:
                        Program.Error(arg3.ErrorReason + " [Exception]");
                        break;
                    case InteractionCommandError.Unsuccessful:
                        Program.Error(arg3.ErrorReason + " [Unsuccefsul]");
                        break;
                    default:
                        Program.Error(arg3.ErrorReason);
                        break;
                }
            }
            return Task.CompletedTask;
        }
        private async Task HandleInteraction(SocketInteraction arg)
        {
            try
            {
                var ctx = new SocketInteractionContext(_client, arg);
                await _interactionCommands.ExecuteCommandAsync(ctx, _services);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (arg.Type == InteractionType.ApplicationCommand)
                    await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }
    }
}