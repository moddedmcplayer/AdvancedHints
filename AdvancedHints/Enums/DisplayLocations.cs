// -----------------------------------------------------------------------
// <copyright file="DisplayLocations.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace AdvancedHints.Enums
{
    /// <summary>
    /// Represents locations on a user's display.
    /// </summary>
    public enum DisplayLocation
    {
        /// <summary>
        /// Represents the top of the screen.
        /// </summary>
        Top,

        /// <summary>
        /// Represents between the <see cref="Middle"/> and the <see cref="Top"/>.
        /// </summary>
        MiddleTop,

        /// <summary>
        /// Represents the middle of the screen.
        /// </summary>
        Middle,

        /// <summary>
        /// Represents between the <see cref="Middle"/> and the <see cref="Bottom"/>.
        /// </summary>
        MiddleBottom,

        /// <summary>
        /// Represents the bottom of the screen.
        /// </summary>
        Bottom,
    }
}