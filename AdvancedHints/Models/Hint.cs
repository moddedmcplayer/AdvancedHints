// -----------------------------------------------------------------------
// <copyright file="Hint.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace AdvancedHints.Models
{
    /// <summary>
    /// Represents a hint to be displayed to a player.
    /// </summary>
    public struct Hint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hint"/> struct.
        /// </summary>
        /// <param name="content"><inheritdoc cref="Content"/></param>
        /// <param name="duration"><inheritdoc cref="Duration"/></param>
        public Hint(string content, float duration)
        {
            Content = content;
            Duration = duration;
        }

        /// <summary>
        /// Gets or sets the content of the hint.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the duration of the hint.
        /// </summary>
        public float Duration { get; set; }
    }
}