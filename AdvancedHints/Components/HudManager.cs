// -----------------------------------------------------------------------
// <copyright file="HudManager.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace AdvancedHints.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AdvancedHints.Enums;
    using AdvancedHints.Models;
    using Exiled.API.Features;
    using UnityEngine;

    /// <summary>
    /// Manages hints on a player's display.
    /// </summary>
    public class HudManager : MonoBehaviour
    {
        private object[] toFormat;
        private Player player;
        private float globalTimer;
        private string hint;

        /// <summary>
        /// Initializes a new instance of the <see cref="HudManager"/> class. - blablabal
        /// Why are you running this, idiot?
        /// Let unity handle it
        /// </summary>
        public HudManager()
        {
            try
            {
                HudManagerInitialized(this);
            }
            catch (Exception e)
            {
                Log.Error($"An error occurred while running {nameof(HudManagerInitialized)}: {e}");
            }
        }

        /// <summary>
        /// Gets all instance of the <see cref="HudManager"/> component.
        /// </summary>
        public static Dictionary<Player, HudManager> Instances { get; } = new Dictionary<Player, HudManager>();

        /// <summary>
        /// Gets or sets a delegate that is called when a <see cref="HudManager"/> is initialized.
        /// </summary>
        public static Action<HudManager> HudManagerInitialized { get; set; } = _ => { };

        /// <summary>
        /// Gets all attached <see cref="HudDisplay"/> instances.
        /// </summary>
        public SortedList<DisplayLocation, HudDisplay> Displays { get; } =
            new SortedList<DisplayLocation, HudDisplay>
            {
                [DisplayLocation.Top] = new HudDisplay(),
                [DisplayLocation.MiddleTop] = new HudDisplay(),
                [DisplayLocation.Middle] = new HudDisplay(),
                [DisplayLocation.MiddleBottom] = new HudDisplay(),
                [DisplayLocation.Bottom] = new HudDisplay(),
            };

        /// <summary>
        /// Gets or sets a value indicating whether debug mode is enabled.
        /// </summary>
        public bool Debug { get; set; } = false;

        /// <summary>
        /// Displays a hint to a player.
        /// </summary>
        /// <param name="player">The player to show the hint to.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="duration">The duration of the hint.</param>
        /// <param name="overrideCurrent">Overrides the active hint.</param>
        /// <param name="displayLocation">The location to display the hint.</param>
        public static void ShowHint(Player player, string message, float duration = 3f, bool overrideCurrent = true, DisplayLocation displayLocation = DisplayLocation.MiddleBottom)
        {
            if (Instances.TryGetValue(player, out HudManager hudManager))
                hudManager.Displays[displayLocation].Enqueue(message, duration, overrideCurrent);
        }

        /// <summary>
        /// Loads the configs default messages.
        /// </summary>
        public void LoadConfig()
        {
            foreach (var kvp in Displays)
            {
                if (Plugin.Singleton.Config.DefaultMessages.TryGetValue(kvp.Key, out var defaultText))
                    kvp.Value.DefaultText = defaultText;
            }
        }

        private void Start()
        {
            player = Player.Get(gameObject);
            Instances.Add(player, this);
            LoadConfig();
        }

        private void FixedUpdate()
        {
            globalTimer += Time.fixedDeltaTime;
            if (globalTimer > Plugin.Singleton.Config.RefreshRate)
            {
                UpdateHints();
                globalTimer = 0;
            }
        }

        private void OnDestroy()
        {
            Instances.Remove(player);
            foreach (var display in Displays.Values)
                display.Kill();
        }

        private void UpdateHints()
        {
            toFormat = Displays.Values.Select(display => FormatStringForHud(
                display.Content ?? display.DefaultText, Plugin.Singleton.Config.LinesPerPosition))
                .ToArray<object>();

            string newHint = string.Format(Plugin.Singleton.Config.HudTemplate, toFormat);
            if (Debug && newHint != hint)
                player.SendConsoleMessage(newHint, "white");
            hint = newHint;

            player.ShowHint(Plugin.HintPrefix + hint, Plugin.Singleton.Config.HintDuration);
        }

        private string FormatStringForHud(string text, int needNewLine)
        {
            int curNewLine = text.Count(x => x == '\n');
            for (int i = 0; i < needNewLine - curNewLine; i++)
                text += '\n';
            return text;
        }
    }
}