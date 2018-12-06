using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings {
    
    //scene data
    public static int FloorLength {get; set;}
    public static int MaxNumberOfFlyingObjects {get; set;}

    //player data
	public static float PlayerSpeed {get; set;}

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

    //sky data
    public static float SkySpeed{get; set;}

    //water "ocean" data
    //wave A
    public static float WaveADirX {get; set;}
    public static float WaveADirZ {get; set;}
    public static float WaveASteepless {get; set;}
    public static float WaveALength {get; set;}
    //wave B
    public static float WaveBDirX {get; set;}
    public static float WaveBDirZ {get; set;}
    public static float WaveBSteepless {get; set;}
    public static float WaveBLength {get; set;}
    //wave C
    public static float WaveCDirX {get; set;}
    public static float WaveCDirZ {get; set;}
    public static float WaveCSteepless {get; set;}
    public static float WaveCLength {get; set;}

    public static float WavesHeight {get; set;}

    //water "mountain" data
    //wave A
    public static float WaveAAplitude {get; set;}
    public static float WaveAFrequency {get; set;}
    //public static float WaveASteepless{get; set;}
    public static float WaveASpeed {get; set;}
    //public static float WaveADirX {get; set;}
    //public static float WaveADirZ {get; set;}
    //wave B
    public static float WaveBAplitude {get; set;}
    public static float WaveBFrequency {get; set;}
    //public static float WaveBSteepless{get; set;}
    public static float WaveBSpeed {get; set;}
    //public static float WaveBDirX {get; set;}
    //public static float WaveBDirZ {get; set;}
    //wave C
    public static float WaveCAplitude {get; set;}
    public static float WaveCFrequency {get; set;}
    //public static float WaveCSteepless{get; set;}
    public static float WaveCSpeed {get; set;}
    //public static float WaveCDirX {get; set;}
    //public static float WaveCDirZ {get; set;}
    //wave D
    public static float WaveDAplitude {get; set;}
    public static float WaveDFrequency {get; set;}
    public static float WaveDSteepless{get; set;}
    public static float WaveDSpeed {get; set;}
    public static float WaveDDirX {get; set;}
    public static float WaveDDirZ {get; set;}
}
