using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Spell : IUseable , ICastable//, IMoveable ,
{
    [SerializeField]
    private string title;

    [SerializeField]
    private int damage;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float castTime;

    [SerializeField]
    private GameObject spellPrefab;

    [SerializeField]
    private Color barColor;

    public string MyTitle { get => title; set => title = value; }

    public int MyDamage { get => damage; set => damage = value; }

    public Sprite MyIcon { get => icon; set => icon = value; }

   // public Sprite MyICon => throw new NotImplementedException();

    public float MySpeed { get => speed; set => speed = value; }

    public float MyCastTime { get => castTime; set => castTime = value; }

    public GameObject MySpellPrefab { get => spellPrefab; set => spellPrefab = value; }

    public Color MyBarColor { get => barColor; set => barColor = value; }

   

    public void Use()
    {
         
    }
}