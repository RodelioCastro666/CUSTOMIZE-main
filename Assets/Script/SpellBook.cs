using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{

    private static SpellBook instance;

    public static SpellBook MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<SpellBook>();
            }

            return instance;
        }
    }

    [SerializeField]
    private Image castingBar;

    [SerializeField]
    private TextMeshProUGUI spellName;

    [SerializeField]
    private TextMeshProUGUI castTime;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Spell[] spells;

    private Coroutine spellRoutine;

    private Coroutine fadeRoutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Spell CastSpell(int index)
    {
        castingBar.color = spells[index].MyBarColor;

        castingBar.fillAmount = 0;

        spellName.text = spells[index].MyTitle;

        icon.sprite = spells[index].MyIcon;

        spellRoutine = StartCoroutine(Progress(index));

        fadeRoutine = StartCoroutine(FadeBar());

        return spells[index];
    }

    public void  Cast(ICastable castable)
    {
        castingBar.color = castable.MyBarColor;

        castingBar.fillAmount = 0;

        spellName.text = castable.MyTitle;

        icon.sprite = castable.MyIcon;

        spellRoutine = StartCoroutine(ProgressICast(castable));

        fadeRoutine = StartCoroutine(FadeBar());

       
    }

    private IEnumerator ProgressICast(ICastable castable)
    {
        float timePassed = Time.deltaTime;

        float rate = 1.0f / castable.MyCastTime;

        float progress = 0.0f;

        while (progress <= 1.0)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;

            castTime.text = (castable.MyCastTime - timePassed).ToString("F2");

            if (castable.MyCastTime - timePassed < 0)
            {
                castTime.text = "0.00";
            }

            yield return null;
        }

        StopCasting();
    }

    private IEnumerator Progress(int index)
    {
        float timePassed = Time.deltaTime;

        float rate = 1.0f / spells[index].MyCastTime;

        float progress = 0.0f;

        while(progress <= 1.0)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;

            castTime.text = (spells[index].MyCastTime - timePassed).ToString("F2");

            if (spells[index].MyCastTime - timePassed < 0)
            {
                castTime.text = "0.00";
            }

            yield return null;
        }

        StopCasting();
    }

    private IEnumerator FadeBar()
    {
        

        float rate = 1.0f / 0.50f;

        float progress = 0.0f;

        while (progress <= 1.0)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            

            yield return null;
        }
    }

    public void StopCasting()
    {
        if(fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            canvasGroup.alpha = 0;
            fadeRoutine = null;
        }
        if(spellRoutine != null)
        {
            StopCoroutine(spellRoutine);
            spellRoutine = null;
        }
    }

    public Spell GetSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.MyTitle == spellName);

        return spell;
    }
}
