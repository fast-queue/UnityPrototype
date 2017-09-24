using UnityEngine;
using System.Collections;

public class Queue : FQ.BaseBody{
    public int maxPlayers;
    public string name;

    // Initialize the queue
    public Queue(string name, int maxPlayers)
    {
        this.name = name;
        this.maxPlayers = maxPlayers;
        // add the queue to the server when it's initilized
        this._id = Manager.Instance.api.addQueue<Queue>(this)._id; // get the id from the server
    }

    // Delete the instance of the queue on server-side when it's deleted
    ~Queue()
    {
        Manager.Instance.api.deleteQueue<Queue>(this);
    }

    // Simple player quantity controll.
    public bool addPlayer(Player player)
    {
        if (Manager.Instance.api.getPlayers<Queue, Player>(this).Length >= this.maxPlayers)
        {
            return false;
        }
        Manager.Instance.api.addPlayer<Queue, Player>(this, player);
        return true;
    }

}
