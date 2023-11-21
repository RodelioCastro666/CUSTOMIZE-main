using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wait : MonoBehaviour
{
    
    private float waitTime = 30;

    private void Start()
    {
        StartCoroutine(waitF());
    }

    IEnumerator waitF()
    {
        yield return new  WaitForSeconds(waitTime);

        SceneManager.LoadScene(1);
    }
}
