using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings {
    
    //scene data
    public static int FloorLength {get; set;}
    public static int MaxNumberOfFlyingObjects {get; set;}

    //player data
	public static float Speed {get; set;}

    //flying objects data
    public static bool Pulse {get; set;}
    public static bool Rotate {get; set;}
    public static float RotateDirectionX {get; set;}
    public static float RotateDirectionY {get; set;}
    public static float RotateDirectionZ {get; set;}
    public static float RotateSpeed {get; set;}
    public static float PulseSpeed {get; set;}
    public static float PulseGrowthBound {get; set;}
    public static float PulseShrinkBound {get; set;}
    public static float PulseCurrentRatio {get; set;}
    public static float RotateAmountX {get; set;}
    public static float RotateAmountY {get; set;}
    public static float RotateAmountZ {get; set;}

}
