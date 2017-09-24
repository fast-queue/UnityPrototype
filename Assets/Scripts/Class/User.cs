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
}

public enum Class : int
{
    Knight = 0,
    Mage,
    Paladin,
    Druid
}