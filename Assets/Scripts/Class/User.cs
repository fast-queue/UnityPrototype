using UnityEngine;
using System.Collections;

public class User {

    public string name;
    public int classe;

    public User(string name, int classe)
    {
        this.name = name;
        this.classe = classe;
    }

    public string getClassText()
    {
        string ret = "Knight";
        switch (classe){
            case (int)Classes.Druid:
                ret = "Druid";
                break;
            case (int)Classes.Knight:
                ret = "Knight";
                break;
            case (int)Classes.Mage:
                ret = "Mage";
                break;
            case (int)Classes.Paladin:
                ret = "Paladin";
                break;
            default:
                ret = "Knight";
                break; 
        }
        return ret;
    }
}

public enum Classes : int
{
    Knight = 0,
    Mage,
    Paladin,
    Druid
}