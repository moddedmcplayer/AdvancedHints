#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1516 // Elements should be separated by blank line

namespace AdvancedHints
{
    using System;
    using AdvancedHints.Components;
    using CommandSystem;
    using Exiled.API.Features;
    using RemoteAdmin;

    [CommandHandler(typeof(ClientCommandHandler))]
    public class DebugCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Player.TryGet(sender, out Player ply))
            {
                if (!HudManager.Instances.TryGetValue(ply, out HudManager hudManager))
                {
                    response = "You do not have a HudManager instance.";
                    return false;
                }

                hudManager.Debug = !hudManager.Debug;
                response = $"Debug mode {(hudManager.Debug ? "enabled" : "disabled")} (check client console).";
                return true;
            }

            response = "You must be a player to use this command.";
            return false;
        }

        public string Command { get; } = "toggleadvhdebug";
        public string[] Aliases { get; } = { "tahd" };
        public string Description { get; } = "Toggles debug mode for AdvancedHints.";
    }
}