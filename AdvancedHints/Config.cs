// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements should be documented

namespace AdvancedHints
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using AdvancedHints.Enums;
    using Exiled.API.Interfaces;

    /// <inheritdoc />
    public class Config : IConfig
    {
        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc />
        public bool Debug { get; set; } = false;

        [Description("The refresh rate of the hint display, in seconds per refresh.")]
        public float RefreshRate { get; set; } = 1f;

        [Description("The duration of a single hint, in seconds.")]
        public float HintDuration { get; set; } = 2f;

        [Description("Whether or not to enable plugin overrides (see below).")]
        public bool EnablePluginOverrides { get; set; } = true;

        // ReSharper disable once CollectionNeverUpdated.Global
        [Description("Messages that will appear if nothing else is quened for the display.")]
        public Dictionary<DisplayLocation, string> DefaultMessages { get; set; } = new ()
        {
        };

        [Description("Plugin overrides for specific hint positions.")]
        public Dictionary<string, DisplayLocation> PluginOverrides { get; set; } = new ()
        {
            { "AdvancedHints", DisplayLocation.MiddleBottom },
        };

        [Description("The template for the hint display.")]
        public string HudTemplate { get; set; } =
            "\"<line-height=95%><voffset=8.5em><alpha=#ff>\\n\\n\\n<align=center>{0}{1}{2}{3}{4}</align>\"";
    }
}