using System;
using Unity.Collections;
using UnityEngine;

namespace Xrofng
{
    /// <summary>
    /// A general countdown timer that tracks duration, allows pausing/resuming,
    /// and triggers an event when the countdown finishes.
    /// </summary>
    [Serializable]
    public class CountdownClock
    {
        /// <summary>
        /// The default countdown time in seconds.
        /// </summary>
        public float Duration;

        /// <summary>
        /// The current timer value counting down to zero.
        /// </summary>
        [ReadOnly]
        public float TimeLeft = 0f;

        /// <summary>
        /// Whether the countdown is currently running.
        /// </summary>
        public bool IsRunning => _isRunning;

        /// <summary>
        /// Returns how much of the countdown has passed, as a 0-1 ratio.
        /// 0 = just started, 1 = complete.
        /// </summary>
        public float Progress01 => Duration > 0f ? Mathf.Clamp01(1f - (TimeLeft / Duration)) : 1f;

        /// <summary>
        /// Alternative name for progress.
        /// </summary>
        public float CompletionRatio => Progress01;

        /// <summary>
        /// Event triggered once when the countdown reaches zero.
        /// </summary>
        public event Action OnCountdownFinished;

        private bool _isRunning = false;

        /// <summary>
        /// Constructor with a default duration.
        /// </summary>
        public CountdownClock(float duration)
        {
            Duration = duration;
        }

        /// <summary>
        /// Call every frame to update the countdown.
        /// </summary>
        public void UpdateTimer(float deltaTime)
        {
            if (!_isRunning)
                return;

            TimeLeft -= deltaTime;

            if (TimeLeft <= 0f)
            {
                TimeLeft = 0f;
                _isRunning = false;
                OnCountdownFinished?.Invoke();
            }
        }

        /// <summary>
        /// Starts or restarts the countdown using the default or overridden duration.
        /// </summary>
        public void StartTimer(float? durationOverride = null)
        {
            Duration = durationOverride ?? Duration;
            TimeLeft = Duration;
            _isRunning = true;
        }

        /// <summary>
        /// Resumes the countdown without resetting the timer.
        /// </summary>
        public void ResumeTimer()
        {
            _isRunning = true;
        }

        /// <summary>
        /// Stops the countdown.
        /// </summary>
        public void StopTimer()
        {
            TimeLeft = 0f;
            _isRunning = false;
        }

        /// <summary>
        /// Stops and resets the countdown to zero.
        /// </summary>
        public void Reset()
        {
            TimeLeft = 0f;
            _isRunning = false;
        }

        /// <summary>
        /// Removes all listeners from the OnCountdownFinished event.
        /// </summary>
        public void ClearOnCountdownFinished()
        {
            OnCountdownFinished = null;
        }
    }
}
