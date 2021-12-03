// -----------------------------------------------------------------------
// <copyright file="HudDisplay.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace AdvancedHints.Models
{
    using System.Collections.Generic;
    using AdvancedHints.Components;
    using MEC;

    /// <summary>
    /// Represents a display part of a <see cref="HudManager"/>.
    /// </summary>
    public class HudDisplay
    {
        private readonly Queue<Hint> queue = new Queue<Hint>();
        private readonly CoroutineHandle coroutineHandle;
        private bool breakNextFrame;

        /// <summary>
        /// Initializes a new instance of the <see cref="HudDisplay"/> class.
        /// </summary>
        public HudDisplay()
        {
            coroutineHandle = Timing.RunCoroutine(HandleDequeue());
        }

        /// <summary>
        /// Gets or sets the content to be displayed.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Enqueues a hint.
        /// </summary>
        /// <param name="content">The content of the hint.</param>
        /// <param name="duration">The duration of the hint.</param>
        /// <param name="overrideQueue">Whether the queue should be cleared before adding the hint.</param>
        public void Enqueue(string content, float duration, bool overrideQueue)
        {
            Hint hint = new Hint(content, duration);
            if (overrideQueue)
            {
                queue.Clear();
                breakNextFrame = true;
            }

            queue.Enqueue(hint);
        }

        /// <summary>
        /// Kills the coroutine that handles the display.
        /// </summary>
        public void Kill()
        {
            Timing.KillCoroutines(coroutineHandle);
        }

        private IEnumerator<float> HandleDequeue()
        {
            while (true)
            {
                if (queue.TryDequeue(out Hint hint))
                {
                    breakNextFrame = false;
                    Content = hint.Content;
                    for (int i = 0; i < 50 * hint.Duration; i++)
                    {
                        if (breakNextFrame)
                        {
                            breakNextFrame = false;
                            break;
                        }

                        yield return Timing.WaitForOneFrame;
                    }

                    Content = string.Empty;
                    continue;
                }

                yield return Timing.WaitForOneFrame;
            }
        }
    }
}