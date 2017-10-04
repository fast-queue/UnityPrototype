using UnityEngine;
using System.Collections;

public class Queue : FQ.BaseBody{
    public int maxPlayers;
    public int numPlayers;
    public string name;

    // Initialize the queue
    public Queue(string name, int maxPlayers)
    {
        this.name = name;
        this.maxPlayers = maxPlayers;
        this.numPlayers = 0;
    }

    // Delete the instance of the queue on server-side when it's deleted

    // Simple player quantity controll.
    public bool addPlayer(Player player)
    {
        if (Manager.Instance.api.getPlayers<Queue, Player>(this).Length >= this.maxPlayers)
        {
            return false;
        }
        numPlayers++;
        Manager.Instance.api.addPlayer<Queue, Player>(this, player);
        return true;
    }

}
