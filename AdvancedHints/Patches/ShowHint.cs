// -----------------------------------------------------------------------
// <copyright file="ShowHint.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace AdvancedHints.Patches
{
#pragma warning disable SA1313
    using AdvancedHints.Components;
    using AdvancedHints.Enums;
    using Exiled.API.Features;
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

            HudManager.ShowHint(__instance, message, duration);
            return false;
        }
    }
}