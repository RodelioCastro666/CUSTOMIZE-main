using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySpell : MonoBehaviour
{
    private void Update()
    {
        DestroyThings();
    }


    public void DestroyThings()
    {
        Destroy(this.gameObject, 2f);
    }
}
