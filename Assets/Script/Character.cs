using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    

    [SerializeField]
    private float speed;

    [SerializeField]
    protected float initiHealth;

    [SerializeField]
    private string type;

    [SerializeField]
    float combatTxtOffset;

    protected Vector2 direction;

    public Transform MyTarget { get; set; }

    public Animator MyAnimator { get; set; }

    protected Rigidbody2D myRigidbody2D;

    protected Coroutine attackRoutine;

    [SerializeField]
    protected Stat health;

    [SerializeField]
    private int level;

    public Stat MyHealth
    {
        get { return health; }
    }

    [SerializeField]
    protected Transform hitBox;

    

    public bool IsAttacking { get; set; }
    public bool IsAttackingSword { get; set; }
    public bool IsAttackingRasen { get; set; }



    public bool IsMoving
    {
        get
        {
            return Direction.x != 0 || Direction.y != 0;
        }
    }

    public Vector2 Direction { get => direction; set => direction = value; }

    public float Speed { get => speed; set => speed = value; }

    public bool IsAlive 
    {
        get
        {
            return health.MyCurrentValue > 0;
        }
    }

    public string MyType { get => type; set => type = value; }


    public int MyLevel { get => level; set => level = value; }

    public float MyCombatTxtOffset { get => combatTxtOffset; set => combatTxtOffset = value; }




    // Start is called before the first frame update
    protected virtual void Start()
    {
       

        myRigidbody2D = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleLayers();

    }

    private void FixedUpdate()
    {
        Move();

       

    }

    public void Move()
    {
        if (IsAlive)
        {
            myRigidbody2D.velocity = Direction.normalized * Speed;

            if (myRigidbody2D.velocity.magnitude > 1f)
            {
                myRigidbody2D.velocity.Normalize();
            }
        }

       
    }

    public void HandleLayers()
    {
        if (IsAlive)
        {
            if (IsAttacking)
            {
                ActivateLayer("Attack");
                Direction = Vector2.zero;


            }
            else if (IsAttackingRasen)
            {
                ActivateLayer("Attack2");
                Direction = Vector2.zero;
            }
            else if (IsAttackingSword)
            {
                ActivateLayer("AttackSword");
                Direction = Vector2.zero;
            }
            else if (IsMoving)
            {

                ActivateLayer("Walk");

                MyAnimator.SetFloat("y", Direction.y);
                MyAnimator.SetFloat("x", Direction.x);

                //StopAttack();
                //StopAttackSword();

            }
            else
            {
                ActivateLayer("Idle");
            }
        }
        else
        {
            ActivateLayer("Death");
        }

        
    }


    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }

        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }

    public virtual void StopAttack()
    {
        IsAttacking = false;
        MyAnimator.SetBool("attack", IsAttacking);
    }

    public virtual void StopAttackSword()
    {
        IsAttackingSword = false;
        MyAnimator.SetBool("attackSword", IsAttackingSword);
    }

    public virtual void StopAttackRasen()
    {
        // StopCoroutine(attackRoutine);
        IsAttackingRasen = false;
        MyAnimator.SetBool("attack2", IsAttackingRasen);
    }

    public virtual void TakeDamage(float damage, Transform source)
    {
        

        health.MyCurrentValue -= damage;
        CombatTextManager.MyInstance.CreateText(transform.position,MyCombatTxtOffset ,damage.ToString(), SCCTYPE.DAMAGE, false);
        if (health.MyCurrentValue <= 0)
        {
            Direction = Vector2.zero;
            myRigidbody2D.velocity = Direction;
            GameManager.MyInstance.OnKillConfirmed(this);
            MyAnimator.SetTrigger("die");

           
        }
    }

    public void GetHealth(int health)
    {
        MyHealth.MyCurrentValue += health;
        CombatTextManager.MyInstance.CreateText(transform.position, MyCombatTxtOffset ,health.ToString(),SCCTYPE.HEAL, true);
    }
}
