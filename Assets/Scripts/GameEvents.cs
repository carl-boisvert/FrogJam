using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public delegate void NewOrderEvent(Order order);
    public static NewOrderEvent OnNewOrderEvent;
    
    public delegate void OrderDoneEvent(Order order);
    public static OrderDoneEvent OnOrderDoneEvent;
    
    public delegate void OrderTimerExpiredEvent(Order order);
    public static OrderTimerExpiredEvent OnOrderTimerExpiredEvent;
    
    public delegate void LookAtRadioEvent();
    public static LookAtRadioEvent OnLookAtRadioEvent;
    
    public delegate void StopLookAtRadioEvent(MusicType musicTypePlaying);
    public static StopLookAtRadioEvent OnStopLookAtRadioEvent;

    public delegate void EnterMusicChannel(MusicType type);
    public static EnterMusicChannel OnEnterMusicChannel;
    
    public delegate void ExitMusicChannel();
    public static ExitMusicChannel OnExitMusicChannel;

    public delegate void FrogChangedMusic();
    public static FrogChangedMusic OnFrogChangedMusic;
    
    public delegate void StoppedFrogMusic();
    public static StoppedFrogMusic OnStoppedFrogMusic;
    
    public delegate void StopLookAtPlantopediaEvent();
    public static StopLookAtPlantopediaEvent OnStopLookAtPlantopediaEvent;
    
    public delegate void GameEndEvent();
    public static GameEndEvent OnGameEndEvent;
    
}
