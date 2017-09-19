using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item : IComparable<Item>
{
    public string myName;
    public int Power;
    public int Defense;
    public string itemSlot;

    protected void CreateEmpty()
    {
        myName = "None";
        Power = 0;
        Defense = 0;
    }
    public override string ToString()
    {
        return myName + "\n" + "Power: " + Power.ToString() + "\n" + "Defense: " + Defense.ToString();
    }

    public int CompareTo(Item other)
    {
        if (other.Power < Power){
            return 0;
        } else if (other.Power == Power){
            return -1;
        } else {
            return 1;
        }
    }

    #region Constructors
    protected Item()
    {
        CreateEmpty();
    }
    protected Item(string name, int pow, int def)
    {
        myName = name;
        Power = pow;
        Defense = def;
    }
    #endregion
}

[Serializable]
public class Helmet : Item
{
    public Helmet()
    {
        CreateEmpty();
        itemSlot = "Helmet";
    }

    public Helmet(string name, int pow, int def) : base(name, pow, def)
    {
        itemSlot = "Helmet";
    }
}

[Serializable]
public class Shoulders : Item
{
    public Shoulders()
    {
        CreateEmpty();
        itemSlot = "Shoulders";
    }

    public Shoulders(string name, int pow, int def) : base(name, pow, def)
    {
        itemSlot = "Shoulders";
    }
}

[Serializable]
public class Torso : Item
{
    public Torso()
    {
        CreateEmpty();
        itemSlot = "Torso";
    }
    public Torso(string name, int pow, int def) : base(name, pow, def)
    {
        itemSlot = "Torso";
    }
}

[Serializable]
public class Legs : Item
{
    public Legs()
    {
        CreateEmpty();
        itemSlot = "Legs";
    }
    public Legs(string name, int pow, int def) : base(name, pow, def)
    {
        itemSlot = "Legs";
    }
}

[Serializable]
public class Boots : Item
{
    public Boots()
    {
        CreateEmpty();
        itemSlot = "Boots";
    }
    public Boots(string name, int pow, int def) : base(name, pow, def)
    {
        itemSlot = "Boots";
    }
}

[Serializable]
public class Gloves : Item
{
    public Gloves ()
    {
        CreateEmpty();
        itemSlot = "Gloves";
    }
    public Gloves(string name, int pow, int def) : base(name, pow, def)
    {
        itemSlot = "Gloves";
    }
}

[Serializable]
public class Equipment
{
    public Helmet myHelmet;
    public Shoulders myShoulders;
    public Torso myTorso;
    public Gloves myGloves;
    public Legs myLegs;
    public Boots myBoots;

    public override string ToString()
    {
        return myHelmet.ToString() + "\n\n" + myShoulders.ToString() + "\n\n" + myTorso.ToString()
            + "\n\n" + myGloves.ToString() + "\n\n" + myLegs.ToString() + "\n\n" + myBoots.ToString() + "\n\n" + "Power: " + GetTotalPower() + "\n" + "Defense: " + GetTotalDefense() + "\n";
    }
    public int GetTotalPower()
    {
        return myHelmet.Power + myShoulders.Power + myTorso.Power + myGloves.Power + myLegs.Power + myBoots.Power;
    }
    public int GetTotalDefense()
    {
        return myHelmet.Defense + myShoulders.Defense + myTorso.Defense + myGloves.Defense + myLegs.Defense + myBoots.Defense;
    }

    public Item SwapItem(Item slot)
    {
        Item oldItem = null;
        if (slot.GetType().Equals(myHelmet.GetType()))
        {
            oldItem = myHelmet;
            myHelmet = (Helmet) slot;
        }
        else if (slot.GetType().Equals(myShoulders.GetType()))
        {
            oldItem = myShoulders;
            myShoulders = (Shoulders) slot;
        }
        else if (slot.GetType().Equals(myTorso.GetType()))
        {
            oldItem = myTorso;
            myTorso = (Torso) slot;
        }
        else if (slot.GetType().Equals(myGloves.GetType()))
        {
            oldItem = myGloves;
            myGloves = (Gloves) slot;
        }
        else if (slot.GetType().Equals(myLegs.GetType()))
        {
            oldItem = myLegs;
            myLegs = (Legs) slot;
        }
        else if (slot.GetType().Equals(myBoots.GetType()))
        {
            oldItem = myBoots;
            myBoots = (Boots) slot;
        }
        return oldItem;
    }
    #region Constructors
    public Equipment()
    {
        myHelmet = new Helmet();
        myShoulders = new Shoulders();
        myTorso = new Torso();
        myGloves = new Gloves();
        myLegs = new Legs();
        myBoots = new Boots();
    }

    public Equipment(Helmet helm, Shoulders shoulders, Torso torso, Gloves gloves, Legs legs, Boots boots)
    {
        myHelmet = helm;
        myShoulders = shoulders;
        myTorso = torso;
        myGloves = gloves;
        myLegs = legs;
        myBoots = boots;
    }
    #endregion
}
