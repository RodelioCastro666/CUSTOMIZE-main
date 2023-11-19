using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SCCTYPE { DAMAGE,HEAL,XP}

public class CombatTextManager : MonoBehaviour
{
    private static CombatTextManager instance;

    public static CombatTextManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<CombatTextManager>();
            }

            return instance;
        }
    }

    

    [SerializeField]
    private GameObject combatTxtPrefab;

    public void CreateText(Vector2 position, float offset, string text, SCCTYPE type, bool crit)
    {
        position.y += offset;
        Text sct =  Instantiate(combatTxtPrefab, transform).GetComponent<Text>();
        sct.transform.position = position;

        string before = string.Empty;
        string after = string.Empty;
        switch (type) 
        {
            case SCCTYPE.DAMAGE:
                before = "-";
                sct.color = Color.red;
                break;
            case SCCTYPE.HEAL:
                before = "+";
                sct.color = Color.green;
                break;
            case SCCTYPE.XP:
                before = "+";
                after = " XP";
                sct.color = Color.yellow;
                break;
        }
        sct.text = before + text + after;


        if (crit)
        {
            sct.GetComponent<Animator>().SetBool("crit", crit);
        }

    }
}
