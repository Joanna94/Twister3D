using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Scene {

    public int FloorLength {get; set;}
    public int MaxNumberOfFlyingObjects {get; set;}
    public List<FlyingObjects> FlyingObjects {get; set;}
    public Player Player {get; set;}
}
