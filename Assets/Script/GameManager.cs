using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void KillConfirmed(Character character);

public class GameManager : MonoBehaviour
{
    public event KillConfirmed killConfirmedEvent;

    [SerializeField]
    private Player player;

    private Camera mainCamera;

    private static GameManager instance;

    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();
            }

            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;   
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();
    }

    private void ClickTarget()
    {
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
           
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if(hit.collider != null)
            {
                IInteractable entity = hit.collider.gameObject.GetComponent<IInteractable>();

                if (hit.collider != null && (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable") && player.MyInteractables.Contains(entity)) 
                {
                    entity.Interact();

                }
            }

           
        }
    }

    public void OnKillConfirmed(Character character)
    {
        if(killConfirmedEvent != null)
        {
            killConfirmedEvent(character);
        }
    }
}
