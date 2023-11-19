using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="HealthPotion", menuName ="Items/Potion", order =2)]
public class HealthPotion : Item, IUseable
{
    [SerializeField]
    private int health;

    public void Use()
    {
        if(Player.MyInstance.MyHealth.MyCurrentValue < Player.MyInstance.MyHealth.MyMaxValue)
        {
            Remove();

            Player.MyInstance.GetHealth(health);
        }

       
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n Restore {0} health", health);
    }
}
