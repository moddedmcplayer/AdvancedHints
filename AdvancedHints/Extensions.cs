// -----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace AdvancedHints
{
    using AdvancedHints.Components;
    using AdvancedHints.Enums;
    using Exiled.API.Features;

    /// <summary>
    /// Various extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <inheritdoc cref="HudManager.ShowHint"/>
        public static void ShowManagedHint(this Player player, string message, float duration = 3f, bool overrideQueue = true, DisplayLocation displayLocation = DisplayLocation.MiddleBottom) =>
            HudManager.ShowHint(player, message, duration, overrideQueue, displayLocation);
    }
}