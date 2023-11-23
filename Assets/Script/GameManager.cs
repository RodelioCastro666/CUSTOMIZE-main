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

    [SerializeField]
    private GameObject indicateW;

    [SerializeField]
    private LayerMask clickableLayer, groundLayer;

    private Camera mainCamera;

    public List<TouchLocation> touches = new List<TouchLocation>();

    private HashSet<Vector3Int> blocked = new HashSet<Vector3Int>();

    public HashSet<Vector3Int> Blocked { get => blocked; set => blocked = value; }

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


        //for (int i = 0; i < Input.touchCount; i++)
        //{
        //    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.touches[1].position);

        //    if(Input.touchCount == 1)
        //    {

        //    }
        //}
        ClickTarget();
        
    }



    public void FixedUpdate()
    {
      



    }

    public void ClickTarget()
    {

        //if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject())
        //{

        //    Touch touch = Input.GetTouch(0);
        //    RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(touch.position), Vector2.zero, Mathf.Infinity, clickableLayer);

        //    if (hit.collider != null)
        //    {
        //        IInteractable entity = hit.collider.gameObject.GetComponent<IInteractable>();

        //        if (hit.collider != null && (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable") && player.MyInteractables.Contains(entity))
        //        {
        //            entity.Interact();
        //            //  indicateW.SetActive(true);

        //        }
        //        else
        //        {
        //            hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(touch.position), Vector2.zero, Mathf.Infinity, groundLayer);

        //            if (hit.collider != null)
        //            {
        //                player.GetPath(mainCamera.ScreenToWorldPoint(touch.position));
        //            }
        //        }

        //    }
        //}



        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableLayer);

            if (hit.collider != null)
            {
                IInteractable entity = hit.collider.gameObject.GetComponent<IInteractable>();
                if (hit.collider != null && (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable") && player.MyInteractables.Contains(entity))
                {
                    entity.Interact();
                    Debug.Log("ETOoo");

                }

            }
            else
            {
                hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, groundLayer);

                if (hit.collider != null)
                {
                    player.GetPath(mainCamera.ScreenToWorldPoint(Input.mousePosition));
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

    //public void OnPointerClick(PointerEventData eventData)
    //{

    //    if(eventData.button == PointerEventData.InputButton.Left)
    //    {
    //        RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

    //        if (hit.collider != null)
    //        {
    //            IInteractable entity = hit.collider.gameObject.GetComponent<IInteractable>();

    //            if (hit.collider != null && (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable") && player.MyInteractables.Contains(entity))
    //            {
    //                entity.Interact();

    //            }

    //        }
    //    }

      
    ////}
}
