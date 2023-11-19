using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public delegate void HealthChanged(float health);

public delegate void CharacterRemoved();

public class Enemy :Character, IInteractable
{
    [SerializeField]
    private CanvasGroup healthGroup;

    private static Enemy instance;

    public static Enemy MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<Enemy>();
            }

            return instance;
        }
    }

    private IState currentState;

    public event HealthChanged healthChanged;

    public event CharacterRemoved characterRemoved;

    [SerializeField]
    private float initAggroRange;

    [SerializeField]
    private TextMeshProUGUI lvlText;

    public float MyAggroRange { get; set; }

    public bool InRange
    {
        get
        {
            return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange;
        }
    }


    public float MyAttackTime { get; set; }

    public Vector3 MyStartPosition { get; set; }

    public float MyAttackRange { get; set; }

    

    protected void Awake()
    {
        SpriteRenderer sr;
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = true;
        health.Initialize(initiHealth, initiHealth);
        MyStartPosition = transform.position;
        MyAggroRange = initAggroRange;
        MyAttackRange = 0.5f;
        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        if (IsAlive)
        {
            if (!IsAttacking)
            {
                MyAttackTime += Time.deltaTime;
            }

            currentState.Update();

        }

        base.Update();

        TxtLvl();
    }

    [SerializeField]
    private LootTable lootTable;

    public void OnHealthChanged(float health)
    {
        if (healthChanged != null)
        {
            healthChanged(health);
        }
    }

    public void TxtLvl()
    {

        lvlText.text = MyLevel.ToString();

        if (MyLevel >= Player.MyInstance.MyLevel + 5)
        {
            lvlText.color = Color.red;
        }
        else if (MyLevel == Player.MyInstance.MyLevel + 3 || MyLevel == Player.MyInstance.MyLevel + 4)
        {
            lvlText.color = new Color32(255, 124, 0, 255);
        }
        else if (MyLevel >= Player.MyInstance.MyLevel - 2 && MyLevel <= Player.MyInstance.MyLevel + 2)
        {
            lvlText.color = Color.yellow;
        }
        else if (MyLevel <= Player.MyInstance.MyLevel - 3 && MyLevel > XpManager.CalculateGrayLevel())
        {
            lvlText.color = Color.green;
        }
        else
        {
            lvlText.color = Color.grey;
        }
    }

    public void ChangeState(IState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }

    public override void TakeDamage(float damage, Transform source)
    {
        if(!(currentState is EvadeState))
        {
            if (IsAlive)
            {
                SetTarget(source);

                base.TakeDamage(damage, source);

                OnHealthChanged(health.MyCurrentValue);

                if (!IsAlive)
                {
                    Player.MyInstance.GainXp(XpManager.CalculateXP(this as Enemy));
                }
            }

        }

    }

    public void SetTarget(Transform target)
    {
        if(MyTarget == null && !(currentState is EvadeState))
        {
            float distance = Vector2.Distance(transform.position, target.position);
            MyAggroRange = initAggroRange;
            MyAggroRange += distance;
            MyTarget = target;
        }
    }

    public void Reset()
    {
        this.MyTarget = null;
        this.MyAggroRange = initAggroRange;
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;
        OnHealthChanged(health.MyCurrentValue);
    }

    public void Interact()
    {
        if (!IsAlive)
        {
            List<Drop> drops = new List<Drop>();

            foreach (IInteractable interactable in Player.MyInstance.MyInteractables)
            {
                if (interactable is Enemy && !(interactable as Enemy).IsAlive)
                {
                    drops.AddRange((interactable as Enemy).lootTable.GetLoot());
                }
            }

            LootWindow.MyInstance.CreatePages(drops);
        }

    }

    public  void StopInteract()
    {
        LootWindow.MyInstance.Close();
    }

    public  Transform Select()
    {
        return hitBox;
    }
}
