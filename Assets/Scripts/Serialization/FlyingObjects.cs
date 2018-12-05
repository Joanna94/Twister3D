using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FlyingObjects {

    public string Color {get; set;}
    public float[] Position {get; set;}
    public string Pulse {get; set;}
    public string Rotate {get; set;}
    public float[] RotateDirection {get; set;}
    public float RotateSpeed {get; set;}
    public float PulseSpeed {get; set;}
    public float PulseGrowthBound {get; set;}
    public float PulseShrinkBound {get; set;}
    public float PulseCurrentRatio {get; set;}
    public float[] RotateAmount {get; set;}
}
