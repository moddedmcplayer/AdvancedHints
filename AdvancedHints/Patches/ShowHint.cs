// -----------------------------------------------------------------------
// <copyright file="ShowHint.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace AdvancedHints.Patches
{
#pragma warning disable SA1313
    using System;
    using System.Linq;
    using AdvancedHints.Components;
    using AdvancedHints.Enums;
    using Exiled.API.Features;
    using Exiled.Loader;
    using HarmonyLib;

    /// <summary>
    /// Hijacks <see cref="Player.ShowHint"/> and directs it towards <see cref="ShowHint"/>.
    /// </summary>
    [HarmonyPatch(typeof(Player), nameof(Player.ShowHint), typeof(string), typeof(float))]
    internal static class ShowHint
    {
        private static bool Prefix(Player __instance, string message, float duration = 3f)
        {
            if (message.Contains("You will respawn in"))
            {
                HudManager.ShowHint(__instance, "\n" + message, duration, displayLocation: DisplayLocation.Middle);
                return false;
            }

            DisplayLocation displayLocation = DisplayLocation.MiddleBottom;

            if (Plugin.Singleton.Config.EnablePluginOverrides)
            {
                try
                {
                    var stackTrace = new System.Diagnostics.StackTrace();
                    if (stackTrace.FrameCount >= 3)
                    {
                        var caller = stackTrace.GetFrame(2).GetMethod().DeclaringType?.Assembly;
                        if (caller != null)
                        {
                            var plugin = Loader.Plugins.FirstOrDefault(x => x?.Assembly == caller);
                            if (plugin != null && !string.IsNullOrWhiteSpace(plugin.Prefix))
                            {
                                Log.Debug($"Plugin with prefix {plugin.Prefix} showing hint {message}");
                                if (Plugin.Singleton.Config.PluginOverrides?.TryGetValue(
                                        plugin.Prefix,
                                        out var displayLocation2) ?? false)
                                {
                                    Log.Debug($"Found override {displayLocation2}");
                                    displayLocation = displayLocation2;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Debug("Patch override: " + e.GetType());
                }
            }

            HudManager.ShowHint(__instance, message, duration, true, displayLocation);
            return false;
        }
    }
}