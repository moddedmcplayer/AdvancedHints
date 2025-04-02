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
    using System.Reflection;
    using AdvancedHints.Components;
    using AdvancedHints.Enums;
    using Exiled.API.Features;
    using Exiled.Loader;
    using HarmonyLib;
    using Hints;
    using Hint = Hints.Hint;

    /// <summary>
    /// Hijacks <see cref="HintDisplay.Show"/> and directs it towards <see cref="ShowHint"/>.
    /// </summary>
    [HarmonyPatch(typeof(HintDisplay), nameof(HintDisplay.Show))]
    internal static class ShowHint
    {
        private static bool Prefix(HintDisplay __instance, Hint hint)
        {
            if (hint is not TextHint textHint || !Player.TryGet(__instance.gameObject, out Player player))
                return true;

            if (textHint.Text.StartsWith(Plugin.HintPrefixSkip))
            {
                textHint.Text = textHint.Text.Substring(Plugin.HintPrefixSkip.Length);
                if (textHint.Parameters.ElementAtOrDefault(0) is StringHintParameter stringHintParameter)
                {
                    if (stringHintParameter.Value.StartsWith(Plugin.HintPrefixSkip))
                        stringHintParameter.Value = stringHintParameter.Value.Substring(Plugin.HintPrefixSkip.Length); // idk if this is necessary
                }

                return true;
            }

            ProcessHint(player, textHint.Text, textHint.DurationScalar);
            return false;
        }

        private static void ProcessHint(Player player, string message, float duration)
        {
            DisplayLocation displayLocation = DisplayLocation.MiddleBottom;

            if (Plugin.Singleton.Config.EnableMessageStartsWithOverrides)
            {
                foreach (var kvp in Plugin.Singleton.Config.MessageStartsWithOverrides)
                {
                    if (message.Contains(kvp.Key))
                    {
                        displayLocation = kvp.Value;
                        break;
                    }
                }
            }

            if (Plugin.Singleton.Config.EnablePluginOverrides)
            {
                try
                {
                    var stackTrace = new System.Diagnostics.StackTrace();
                    if (stackTrace.FrameCount >= 3)
                    {
                        Assembly caller = stackTrace.GetFrame(2).GetMethod().DeclaringType?.Assembly;
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
                    Log.Debug("Patch override error: " + e.GetType());
                }
            }

            HudManager.ShowHint(player, message, duration, true, displayLocation);
        }
    }
}