using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for handling automated behavior in Unity.
/// Provides lifecycle control for automation, including start delays,
/// process checks, and start/stop state handling.
/// </summary>
public class Automation : BetterMonoBehaviour
{
    // Whether to automatically start the automation when the scene starts
    public bool AutomateOnStart = true;

    // Delay before starting the automation (after Start is called)
    [SerializeField] public float OnStartDelay = 0;

    // Delay before ending the automation (currently unused, but reserved for future logic)
    [SerializeField] public float OnEndDelay = 0;

    // Flag indicating whether this instance is allowed to run automation
    public bool IsAutomationStarted { get; private set; }

    // Stores the initial position of the GameObject (useful for resetting)
    protected Vector3 initialPos;

    // Tracks whether the automation is currently running
    protected bool automating;

    protected override void Start()
    {
        base.Start();
        // Record the initial position for reference or reset
        initialPos = transform.position;

        // Allow subclasses to perform custom setup
        Initialization();

        // If auto-start is enabled, begin automation after delay or immediately
        if (AutomateOnStart)
        {
            if (OnStartDelay > 0)
            {
                // Wait for the specified delay before starting automation
                StartCoroutine(StartRoutine());
            }
            else
            {
                InvokeAutomation();
            }
        }
    }

    /// <summary>
    /// Called during Start(). Intended to be overridden for subclass-specific setup.
    /// </summary>
    protected virtual void Initialization()
    {
    }

    /// <summary>
    /// Resets the GameObject’s position back to its initial recorded position.
    /// </summary>
    public virtual void ResetPosition()
    {
        transform.position = initialPos;
    }

    /// <summary>
    /// Coroutine to delay automation start.
    /// </summary>
    private IEnumerator StartRoutine()
    {
        yield return new WaitForSeconds(OnStartDelay);
        StartAutomation();
        DoAutomation();
    }

    /// <summary>
    /// Invokes automation manually, checking conditions before running.
    /// </summary>
    public void InvokeAutomation()
    {
        StartAutomation();
        if (CheckAutomationInvokeCondition())
        {
            DoAutomation();
        }
    }

    /// <summary>
    /// Condition check before automation is invoked. Override to define custom conditions.
    /// </summary>
    protected virtual bool CheckAutomationInvokeCondition()
    {
        return true;
    }

    /// <summary>
    /// Main entry point for executing automation logic. 
    /// Override this to define what happens when automation begins.
    /// </summary>
    protected virtual void DoAutomation()
    {
    }

    /// <summary>
    /// Called when automation is stopped. Override to perform cleanup or transitions.
    /// </summary>
    protected virtual void OnAutomationStop()
    {
    }

    /// <summary>
    /// Starts the automation and triggers OnStarted().
    /// </summary>
    public void StartAutomation()
    {
        IsAutomationStarted = true;
        OnStarted();
    }

    /// <summary>
    /// Called when automation starts. Override for setup logic.
    /// </summary>
    protected virtual void OnStarted()
    {
    }

    /// <summary>
    /// Stops the automation and revokes its active state.
    /// </summary>
    public void StopAutomation()
    {
        IsAutomationStarted = false;
        OnAutomationStop();
    }

    /// <summary>
    /// Checks if automation should continue running each frame.
    /// Override this to define conditions like distance, state, or triggers.
    /// </summary>
    protected virtual bool CheckAutomationProcessCondition()
    {
        return true;
    }

    /// <summary>
    /// Main per-frame logic for automation. 
    /// Override this to implement behavior like movement or repeated actions.
    /// </summary>
    protected virtual void ProcessAutomation()
    {
    }

    private void Update()
    {
        // If started and conditions are met, run automation
        if (IsAutomationStarted && CheckAutomationProcessCondition())
        {
            ProcessAutomation();
            automating = true;
        }
        // If automation was running but conditions no longer hold, stop it
        else if (automating)
        {
            StopAutomation();
            automating = false;
        }
    }
}
