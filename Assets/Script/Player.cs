using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : Character
{
    private static Player instance;

    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<Player>();
            }

            return instance;
        }
    }

    [SerializeField]
    private FixedJoystick JoyStick;

    [SerializeField]
    private Stat mana;

    [SerializeField]
    private Stat xpStat;

    [SerializeField]
    private TextMeshProUGUI lvlText;

    private List<IInteractable> interactables = new List<IInteractable>();

    private float initiMana = 50;

    public Coroutine MyInitRoutine { get; set; }

    [SerializeField]
    private Transform[] exitPoints;

    [SerializeField]
    private Animator ding;

    private int exitIndex = 2;

    private SpellBook spellBook;

    public int MyGold { get; set; }

    public List<IInteractable> MyInteractables { get => interactables; set => interactables = value; }

    public Stat MyXp { get => xpStat; set => xpStat = value; }

    public Stat MyMana { get => mana; set => mana = value; }

    private Vector3 min, max;

    protected override void Start()
    {

       
        spellBook = GetComponent<SpellBook>();
       
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {

        GetInput();

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x),
            Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);

        base.Update();

        

    }

    public void SetDefaultValues()
    {
        MyGold = 10000;
        health.Initialize(initiHealth, initiHealth);
        MyMana.Initialize(initiMana, initiMana);
        MyXp.Initialize(0, Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f)));
        lvlText.text = MyLevel.ToString();
        //initPos = transform.parent.position;
    }



    private void GetInput()
    {
        direction = Vector2.zero;




        //direction.x = Input.GetAxisRaw("Horizontal");
       // direction.y = Input.GetAxisRaw("Vertical");


        direction.x = JoyStick.Horizontal;
        direction.y = JoyStick.Vertical;


        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            direction.y = 0;
            if (direction.x > 0)
            {
                Debug.Log("right");
                exitIndex = 1;

            }

            if (direction.x < 0)
            {
                Debug.Log("left");
                exitIndex = 3;

            }
        }
        else
        {
            direction.x = 0;

            if (direction.y > 0)
            {
                Debug.Log("up");
                exitIndex = 0;

            }

            if (direction.y < 0)
            {
                Debug.Log("down");
                exitIndex = 2;

            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GainXp(600);
        }

        if (Input.GetKey(KeyBindManager.MyInstacne.Keybinds["UP"]))
        {
            Direction += Vector2.up;
        }
        if (Input.GetKey(KeyBindManager.MyInstacne.Keybinds["LEFT"]))
        {
            Direction += Vector2.left;
        }
        if (Input.GetKey(KeyBindManager.MyInstacne.Keybinds["DOWN"]))
        {
            Direction += Vector2.down;
        }
        if (Input.GetKey(KeyBindManager.MyInstacne.Keybinds["RIGHT"]))
        {
            Direction += Vector2.right;
        }

        foreach(string action in KeyBindManager.MyInstacne.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeyBindManager.MyInstacne.ActionBinds[action]))
            {
                UiManager.MyInstance.CLickActionButton(action);
            }
        }


        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
            MyMana.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            MyMana.MyCurrentValue += 10;
        }      
        
    }

    public void SetLimits(Vector3 min,Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    private void StopInit()
    {
        if (MyInitRoutine != null)
        {
            StopCoroutine(MyInitRoutine);
        }
    }

    private IEnumerator Attack()
    {
        Spell newSpell = spellBook.CastSpell(0);

        if (!IsAttacking)
        {
            IsAttacking = true;
            MyAnimator.SetBool("attack", true);

            yield return new WaitForSeconds(newSpell.MyCastTime);

            Vector2 temp = new Vector2(MyAnimator.GetFloat("x"), MyAnimator.GetFloat("y"));

            FireSpell spell = Instantiate(newSpell.MySpellPrefab, transform.position, Quaternion.identity).GetComponent<FireSpell>();
            spell.SetUp(temp, ChooseSpellDirection());
            spell.Initialize(newSpell.MyDamage, transform);

            yield return new WaitForSeconds(1);

            StopAttack();
        }

    }

   

    private IEnumerator AttackSword()
    {
        Spell newSpell = spellBook.CastSpell(1);

        if (!IsAttackingSword)
        {
            IsAttackingSword = true;
            MyAnimator.SetBool("attackSword", true);

            yield return new WaitForSeconds(newSpell.MyCastTime);
            Debug.Log("kkk");


            Vector2 temp = new Vector2(MyAnimator.GetFloat("x"), MyAnimator.GetFloat("y"));

            SwordSlash swordSlash = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SwordSlash>();
            swordSlash.SetUp(temp, ChooseSlashDirection());
            swordSlash.Initialize(newSpell.MyDamage,transform);

            yield return new WaitForSeconds(0.5f);

            StopAttackSword();

            
        }
    }

    private IEnumerator AttackRasen()
    {
        Spell newSpell = spellBook.CastSpell(2);

        if (!IsAttackingRasen)
        {
            IsAttackingRasen = true;
            MyAnimator.SetBool("attack2", true);

            yield return new WaitForSeconds(newSpell.MyCastTime);

            Vector2 temp = new Vector2(MyAnimator.GetFloat("x"), MyAnimator.GetFloat("y"));

            RasenSpell rasenSpell = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<RasenSpell>();
            rasenSpell.SetUp(temp, ChooseSlashDirection());
            rasenSpell.Initialize(newSpell.MyDamage,transform);

            yield return new WaitForSeconds(1);

            StopAttackRasen();


        }
    }

    

    Vector3 ChooseSpellDirection()
    {
        float temp = Mathf.Atan2(MyAnimator.GetFloat("y"), MyAnimator.GetFloat("x")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp+180);
    }

    Vector3 ChooseSlashDirection()
    {
        float temp = Mathf.Atan2(MyAnimator.GetFloat("y"), MyAnimator.GetFloat("x")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }

    public void CastRasen()
    {
        

        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !IsAttacking && !IsMoving)
        {
            MyInitRoutine = StartCoroutine(AttackRasen()); 
        }
    }

    public void CastSword()
    {
        

        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !IsAttacking && !IsMoving)
        {
            MyInitRoutine = StartCoroutine(AttackSword()); ;
        }

    }
    public void CastFire()
    {

        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !IsAttacking && !IsMoving)
        {
            MyInitRoutine = StartCoroutine(Attack()); 
        }

    }

    public IEnumerator CraftRoutine(Recipe recipe)
    {
        yield return attackRoutine = StartCoroutine(ActionRoutine());
    }

    private IEnumerator ActionRoutine()
    {
        Spell newSpell = spellBook.CastSpell(2);

        IsAttackingRasen = true;
        IsAttacking = true;
        IsAttackingSword = true;

        MyAnimator.SetBool("attack2", true);
        MyAnimator.SetBool("attack", true);
        MyAnimator.SetBool("attackSword", true);

        yield return new WaitForSeconds(newSpell.MyCastTime);

        StopAttackRasen();
        StopAttack();
        StopAttackSword();


    }

    public void Gather(string skillName, List<Drop> items)
    {
        if (!IsAttacking)
        {
            MyInitRoutine = StartCoroutine(GatherRoutine(items));
        }
    }

    private IEnumerator GatherRoutine( List<Drop> items)
    {
        Spell newSpell = spellBook.CastSpell(3);

        if (!IsAttackingRasen)
        {
            IsAttackingRasen = true;
            MyAnimator.SetBool("attack2", IsAttacking);

            yield return new WaitForSeconds(newSpell.MyCastTime);



            LootWindow.MyInstance.CreatePages(items);

            StopAttackRasen();
        }
    }

    public virtual void StopAttack()
    {
        spellBook.StopCasting();

        IsAttacking = false;
        MyAnimator.SetBool("attack", IsAttacking);
    }

    public virtual void StopAttackSword()
    {
        spellBook.StopCasting();

        IsAttackingSword = false;
        MyAnimator.SetBool("attackSword", IsAttackingSword);
    }

    public virtual void StopAttackRasen()
    {
        

        // StopCoroutine(attackRoutine);
        IsAttackingRasen = false;
        MyAnimator.SetBool("attack2", IsAttackingRasen);
        spellBook.StopCasting();
    }

   

    public void GainXp(int xp)
    {
        MyXp.MyCurrentValue += xp;
        CombatTextManager.MyInstance.CreateText(transform.position,MyCombatTxtOffset ,xp.ToString(), SCCTYPE.XP, false);

        if(MyXp.MyCurrentValue >= MyXp.MyMaxValue)
        {
            StartCoroutine(Ding());
        }
    }

    private IEnumerator Ding()
    {
        while (!MyXp.isFull)
        {
            yield return null;
        }

        MyLevel++;
        ding.SetTrigger("Ding");
        lvlText.text = MyLevel.ToString();
        MyXp.MyMaxValue = 100 * MyLevel * Mathf.Pow(MyLevel, 0.5f);
        MyXp.MyMaxValue = Mathf.Floor(MyXp.MyMaxValue);
        MyXp.MyCurrentValue = MyXp.MyOverFlow;
        MyXp.Reset();

        if (MyXp.MyCurrentValue >= MyXp.MyMaxValue)
        {
            StartCoroutine(Ding());
        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            IInteractable interactable = collision.GetComponent<IInteractable>();

            if (!MyInteractables.Contains(interactable))
            {
                MyInteractables.Add(interactable);
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" || collision.tag == "Interactable" )
        {
            if(MyInteractables.Count > 0)
            {
                IInteractable interactable = MyInteractables.Find(x => x == collision.GetComponent<IInteractable>());

                if(interactable != null)
                {
                    interactable.StopInteract();
                }

                MyInteractables.Remove(interactable);
            }

            
        }
    }


    public void UpdateLevel()
    {
        lvlText.text = MyLevel.ToString();
    }
}
