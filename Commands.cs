using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Net;
using System.Runtime.CompilerServices;

namespace DiscordBotLogging
{
    public class Commands : InteractionModuleBase<SocketInteractionContext>
    {
        private void NotSupportedRespond() => RespondAsync("Не поддерживается", ephemeral: true);
        private const RunMode runMode = RunMode.Async;
        [SlashCommand("создать_свой_канал", "Создает канал, которым вы можете управлять. По стандарту макс. число каналов - 5", false, runMode)]
        public async Task CreateChannel()
        {
            NotSupportedRespond();
        }
        [SlashCommand("удалить_свой_канал", "Удаляет ваш канал", false, runMode)]
        public async Task DeleteChannel(SocketGuildChannel Канал)
        {
            NotSupportedRespond();
        }
        [SlashCommand("удалить_свои_каналы", "Удаляет все ваши каналы", false, runMode)]
        public async Task DeleteAllChannels()
        {
            NotSupportedRespond();
        }
        [SlashCommand("получить_свои_каналы", "Выводит список всех ваших каналов", false, runMode)]
        public async Task GetChannels()
        {
            NotSupportedRespond();
        }
    }
}