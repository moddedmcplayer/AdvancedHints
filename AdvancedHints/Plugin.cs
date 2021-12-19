// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace AdvancedHints
{
    using System;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using HarmonyLib;

    /// <summary>
    /// The main plugin class.
    /// </summary>
    public class Plugin : Plugin<Config>
    {
        private EventHandlers eventHandlers;
        private Harmony harmony;

        /// <inheritdoc />
        public override string Author { get; } = "Build";

        /// <inheritdoc />
        public override string Name { get; } = "AdvancedHints";

        /// <inheritdoc />
        public override string Prefix { get; } = "AdvancedHints";

        /// <inheritdoc />
        public override PluginPriority Priority { get; } = PluginPriority.Higher;

        /// <inheritdoc />
        public override Version RequiredExiledVersion { get; } = new Version(4, 1, 2);

        /// <inheritdoc />
        public override Version Version { get; } = new Version(1, 0, 0);

        /// <inheritdoc />
        public override void OnEnabled()
        {
            harmony = new Harmony($"advancedHints.{DateTime.UtcNow.Ticks}");
            harmony.PatchAll();

            eventHandlers = new EventHandlers();
            Exiled.Events.Handlers.Player.Destroying += eventHandlers.OnDestroying;
            Exiled.Events.Handlers.Player.Verified += eventHandlers.OnVerified;
            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Destroying -= eventHandlers.OnDestroying;
            Exiled.Events.Handlers.Player.Verified -= eventHandlers.OnVerified;
            eventHandlers = null;

            harmony.UnpatchAll();
            harmony = null;
            base.OnDisabled();
        }
    }
}