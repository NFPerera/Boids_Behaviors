using System;
using UnityEngine;

namespace _Main.Scripts.DevelopmentUtilities.Extensions
{
    public class TimerExtensions
    {
        private float m_duration;
        private float m_initialDuration;
        private float m_remainingTime;
        private bool m_isRunning;
        private Action m_onTimerComplete;
        private Action<float> m_onTimerUpdate;
        private Action m_initialOnTimerComplete;
        private Action<float> m_initialOnTimerUpdate;

    public TimerExtensions(float p_duration, Action p_onTimerComplete = null, Action<float> p_onTimerUpdate = null)
    {
        this.m_initialDuration = p_duration;
        this.m_duration = p_duration;
        this.m_remainingTime = p_duration;
        this.m_onTimerComplete = p_onTimerComplete;
        this.m_onTimerUpdate = p_onTimerUpdate;
        this.m_initialOnTimerComplete = p_onTimerComplete;
        this.m_initialOnTimerUpdate = p_onTimerUpdate;
    }

    public void Start()
    {
        if (m_isRunning)
        {
            Console.WriteLine("Timer is already running.");
            return;
        }

        m_isRunning = true;
    }

    public void Stop()
    {
        if (!m_isRunning)
        {
            Console.WriteLine("Timer is not running.");
            return;
        }

        m_isRunning = false;
        m_remainingTime = 0f;
        m_onTimerComplete = null;
        m_onTimerUpdate = null;
    }

    public void Pause()
    {
        m_isRunning = false;
    }

    public void Resume()
    {
        if (m_isRunning)
        {
            Console.WriteLine("Timer is already running.");
            return;
        }

        m_isRunning = true;
    }

    public void Reset()
    {
        m_isRunning = false;
        m_duration = m_initialDuration;
        m_remainingTime = m_initialDuration;
        m_onTimerComplete = m_initialOnTimerComplete;
        m_onTimerUpdate = m_initialOnTimerUpdate;
    }

    public float GetRemainingTime()
    {
        return m_remainingTime;
    }

    public float GetElapsedTime()
    {
        return m_duration - m_remainingTime;
    }

    public bool IsRunning()
    {
        return m_isRunning;
    }

    public void Update(float p_deltaTime)
    {
        if (!m_isRunning) return;

        m_remainingTime -= p_deltaTime;
        m_onTimerUpdate?.Invoke(m_remainingTime);

        if (m_remainingTime <= 0)
        {
            m_isRunning = false;
            m_remainingTime = 0;
            m_onTimerComplete?.Invoke();
        }
    }
    }
}