using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    private void Update()
    {
        DestroyThings();
    }
    

    public void DestroyThings()
    {
        Destroy(this.gameObject, 0.2f);
    }


}
