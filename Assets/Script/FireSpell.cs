using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpell : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private int firedamage;

    private Animator anim;
    // private BoxCollider2D boxCollider;

    private Transform source;

    [SerializeField]
    private Rigidbody2D myrigidbody;

    public Transform Mytarget { get; set; }

    public Sprite MyIcon => throw new System.NotImplementedException();

    private void Awake()
    {
        anim = GetComponent<Animator>();
        // boxCollider = GetComponent<BoxCollider2D>();
        myrigidbody = GetComponent<Rigidbody2D>();

    }

    public void Initialize(int damage, Transform source)
    {
        this.firedamage = damage;
        this.source = source;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Hitbox"))
        {
            Character c = collision.GetComponentInParent<Character>();
            c.TakeDamage(firedamage, source);
            anim.SetTrigger("explode");
            myrigidbody.velocity = Vector2.zero;
        }
        

       
    }

    public void SetUp(Vector2 velocity, Vector3 direction)
    {
        myrigidbody.velocity = velocity.normalized * speed;
        transform.rotation = Quaternion.Euler(direction);
        
    }

    public void Deactivate()
    {
       
    }

    public void Use()
    {
        throw new System.NotImplementedException();
    }
}
