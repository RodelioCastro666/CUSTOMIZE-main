using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchLocation
{
    public int touchId;
    public GameManager mouses;


    public TouchLocation(int touchID, GameManager mouse)
    {
        touchId = touchID;
        mouses = mouse;
        
    }
    
}
