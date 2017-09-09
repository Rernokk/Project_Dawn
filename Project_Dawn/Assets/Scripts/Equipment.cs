using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string myName;
    public int Power;
    public int Defense;

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

public class Helmet : Item
{
    public Helmet()
    {
        CreateEmpty();
    }

    public Helmet(string name, int pow, int def) : base(name, pow, def) { }
}

public class Shoulders : Item
{
    public Shoulders()
    {
        CreateEmpty();
    }

    public Shoulders(string name, int pow, int def) : base(name, pow, def) { }
}

public class Torso : Item
{
    public Torso()
    {
        CreateEmpty();
    }
    public Torso(string name, int pow, int def) : base(name, pow, def) { }
}

public class Legs : Item
{
    public Legs()
    {
        CreateEmpty();
    }
    public Legs(string name, int pow, int def) : base(name, pow, def) { }
}

public class Boots : Item
{
    public Boots()
    {
        CreateEmpty();
    }
    public Boots(string name, int pow, int def) : base(name, pow, def) { }
}

public class Gloves : Item
{
    public Gloves ()
    {
        CreateEmpty();
    }
    public Gloves(string name, int pow, int def) : base(name, pow, def) { }
}

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
