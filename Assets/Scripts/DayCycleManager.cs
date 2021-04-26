using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    public bool IsRunning;
    public float BaseTimeValue;

    public int ClockTickCounter;

    public int PeriodTimer;
    public int PeriodCounter;

    public int DayTimer;
    public int DayCounter;

    public float TimeCounter;

    public event Action OnPeriodComplete;
    public event Action OnDayComplete;
    public event Action OnClockTicks;

    void Start()
    {
        TimeCounter = 0f;

        ClockTickCounter = 0;
        PeriodCounter = 0;
        DayCounter = 0;
        GameManager.instance.UI.UpdateDayTimeUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRunning)
        {
            TimeCounter += Time.deltaTime;
            if (TimeCounter >= BaseTimeValue)
            {
                TimeCounter -= BaseTimeValue;
                ClockTicks();
            }
        }
    }

    void ClockTicks()
    {
        OnClockTicks?.Invoke();
        ClockTickCounter += 1;
        if (ClockTickCounter == PeriodTimer)
        {
            ClockTickCounter = 0;
            PeriodTicks();
        }
    }

    void PeriodTicks()
    {
        OnPeriodComplete?.Invoke();

        PeriodCounter += 1;
        if (PeriodCounter == DayTimer)
        {
            PeriodCounter = 0;
            DayTicks();
        }
        GameManager.instance.UI.UpdateDayTimeUI();
    }

    void DayTicks()
    {
        OnDayComplete?.Invoke();
        DayCounter += 1;
        GameManager.instance.UI.UpdateDayTimeUI();
    }
}
