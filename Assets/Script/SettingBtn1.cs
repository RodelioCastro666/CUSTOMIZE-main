using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingBtn1 : MonoBehaviour
{
    public Animator hsettingAnimataor;

    private bool hslideIn = false;

    public void SettingSLideHolder()
    {
        if (hslideIn == false)
        {
            hslideIn = true;
            hsettingAnimataor.SetBool("hSlideIn", hslideIn);
        }
        else if(hslideIn == true)
        {
            hslideIn = false;
            hsettingAnimataor.SetBool("hSlideIn", hslideIn);
        }
    }
}
