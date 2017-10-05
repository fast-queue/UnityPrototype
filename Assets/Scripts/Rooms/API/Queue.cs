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
        if (this.numPlayers >= this.maxPlayers)
        {
            return false;
        }
        numPlayers++;
        // add player on server
        var p = Manager.Instance.api.addPlayer<Queue, Player>(this, player);
        // set user_id on manager
        Manager.Instance.user._id = p._id;

        // update this queue on server
        Manager.Instance.api.updateQueue<Queue>(this);

        return true;
    }

    public bool removePlayer(Player player) {
        // remove player from queue
        Manager.Instance.api.deletePlayer<Queue, Player>(this, player);
        
        // update queue for user num controll
        var _this = Manager.Instance.api.getQueue<Queue>(this);
        // update numPlayers before update
        this.numPlayers = _this.numPlayers;
        if(numPlayers > 0){
            numPlayers --;
        }
        Manager.Instance.api.updateQueue<Queue>(this);
        
        return true;
    }

    public string toJson(){
        return "{ \"name\": \"" + this.name + "\", \" maxPlayers \" : " + this.maxPlayers + " , \" numPlayers\" : " + this.numPlayers + " }";
    }

}
