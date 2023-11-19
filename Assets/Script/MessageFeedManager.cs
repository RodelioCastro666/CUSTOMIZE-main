using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageFeedManager : MonoBehaviour
{

    private static MessageFeedManager instance;

    public static MessageFeedManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<MessageFeedManager>();
            }

            return instance;
        }
    }

    [SerializeField]
    private GameObject messagePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WriteMessage(string message)
    {
        GameObject go = Instantiate(messagePrefab, transform);

        go.GetComponent<TextMeshProUGUI>().text = message;

        go.transform.SetAsFirstSibling();

        Destroy(go, 1.5f);
    }
}
