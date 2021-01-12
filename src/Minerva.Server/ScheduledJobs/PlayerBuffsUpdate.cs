using System;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Elements.Entities;
using Microsoft.Extensions.Logging;
using Minerva.Server.Callbacks;
using Minerva.Server.Entities;
using Minerva.Server.ScheduledJobs.Base;

namespace Minerva.Server.ScheduledJobs
{
    public class PlayerBuffsUpdate
        : ScheduledJob
    {
        private readonly Random _random;

        private ILogger<PlayerBuffsUpdate> Logger { get; }

        public PlayerBuffsUpdate(ILogger<PlayerBuffsUpdate> logger)
            : base(TimeSpan.FromSeconds(1))
        {
            _random = new Random();
            Logger = logger;
        }

        public override async Task Action()
        {
            var callback = new AsyncFunctionCallback<IPlayer>(async (player) =>
            {
                var serverPlayer = player as ServerPlayer;

                if (serverPlayer != null && serverPlayer.IsSpawned && !serverPlayer.IsDead)
                {

                }

                await Task.CompletedTask;
            });

            await Alt.ForEachPlayers(callback);
        }
    }
}