// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace AdvancedHints
{
    using System;
    using AdvancedHints.Components;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using HarmonyLib;

    /// <summary>
    /// The main plugin class.
    /// </summary>
    public class Plugin : Plugin<Config>
    {
        /// <summary>
        /// The prefix that makes the patch ignore the hint.
        /// </summary>
        public const string HintPrefix = "ADVHINTS";
        private EventHandlers eventHandlers;
        private Harmony harmony;

        /// <summary>
        /// Gets the instance of the plugin.
        /// </summary>
        public static Plugin Singleton { get; private set; }

        /// <inheritdoc />
        public override string Author { get; } = "Build, moddedmcplayer";

        /// <inheritdoc />
        public override string Name { get; } = "AdvancedHints";

        /// <inheritdoc />
        public override string Prefix { get; } = "AdvancedHints";

        /// <inheritdoc />
        public override PluginPriority Priority { get; } = PluginPriority.Higher;

        /// <inheritdoc />
        public override Version RequiredExiledVersion { get; } = new Version(8, 0, 0);

        /// <inheritdoc />
        public override Version Version { get; } = new Version(2, 0, 0);

        /// <inheritdoc />
        public override void OnEnabled()
        {
            Singleton = this;
            harmony = new Harmony($"advancedHints.{DateTime.UtcNow.Ticks}");
            harmony.PatchAll();

            eventHandlers = new EventHandlers();
            Exiled.Events.Handlers.Player.Verified += eventHandlers.OnVerified;
            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Verified -= eventHandlers.OnVerified;
            eventHandlers = null;

            harmony.UnpatchAll(harmony.Id);
            harmony = null;
            Singleton = null;
            base.OnDisabled();
        }

        /// <inheritdoc />
        public override void OnReloaded()
        {
            foreach (var kvp in HudManager.Instances)
                kvp.Value.LoadConfig();
        }
    }
}