using System.Collections;
using System.Collections.Generic;
using FQ;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {
    public Text resText;
    public Text textName;
    public Text id;

    private Dictionary<string, MyQueueClass> queues;
    private Dictionary<string, MyPlayerClass> players;

    const string url = "http://fq-api.bovendorp.org";
    const string key = "testkeytoeveryone";

    RestApi api;
    string queue = "";
    string player = "";

    void Start () {
        api = new RestApi (url, key);
        queues = new Dictionary<string, MyQueueClass> ();
        players = new Dictionary<string, MyPlayerClass> ();
    }

    // NAO GOSTEI
    // FALAR COM O ALISSON PRA VER COMO A GENTE PODE RESOLVER
    // O POLYMORFISMO NAO DEVERIA ACEITAR?
    // TODO: 
    private void updateMap<T> (Dictionary<string, T> map, T[] q) where T : BaseBody {
        map.Clear ();
        foreach (var item in q) {
            map.Add (item._id, item);
        }
    }

    public void deletePlayer () {
        if ((queue == "") || (player == "")){
            Debug.Log("No queue and player selected");
            return;
        }

        var deleted = api.deletePlayer (queues[queue], players[player]);
        resText.text = deleted._id;
        player = "";
    }

    public void deleteQueue () {
        if (queue == ""){
            Debug.Log("No queue selected");
            return;
        }
        var deleted = api.deleteQueue (queues[queue]);
        resText.text = deleted._id;
        queue = "";
    }

    public void getPlayers () {
        if (queue == "") {
            resText.text = "No Queue selected";
            Debug.Log ("No Queue selected");
            return;
        }
        MyPlayerClass[] x = api.getPlayers<MyQueueClass, MyPlayerClass> (queues[queue]);
        if ((x == null) || (x.Length == 0)) {
            Debug.Log ("No players in this Queue");
            resText.text = "No players in this Queue";
            return;
        }
        updateMap<MyPlayerClass> (players, x);
        string n = "[";
        player = x[0]._id;
        for (int i = 0; i < x.Length; i++) {
            n += x[i].toJson ();
            if (x.Length - 1 != i)
                n += ",";
            else
                n += "]";
        }
        resText.text = n;
    }
    public void getPlayerInfo () {
        MyPlayerClass x = api.getPlayer<MyQueueClass, MyPlayerClass> (queues[queue], players[player]);

        resText.text = x.toJson ();
    }

    public void getAllQueue () {
        MyQueueClass[] x = api.getAllQueue<MyQueueClass> ();
        if ((x == null) || (x.Length == 0)) {
            Debug.Log ("No queue to show.");
            resText.text = "No Queue";
            return;
        }
        if (x.Length < 0)
            return;

        queue = x[0]._id;

        // does the memory map update
        updateMap<MyQueueClass> (queues, x);

        textName.text = x[0].name;
        id.text = x[0]._id;

        string output = "";
        for (int i = 0; i < x.Length; i++) {
            output += x[0].toJson ();
            if (i != x.Length) {
                output += ",";
            }
        }

        resText.text = output;
    }

    public void getQueue () {
        MyQueueClass x = api.getQueue<MyQueueClass> (queues[queue]);

        resText.text = x.toJson ();

    }

    public void addQueue (string name) {
        MyQueueClass body = new MyQueueClass (name, 32);
        var resp = api.addQueue<MyQueueClass> (body);
        queues.Add (resp._id, resp);

        // set text on editor
        id.text = resp._id;
        textName.text = resp.name;
    }

    public void addPlayer (string name) {
        MyPlayerClass player = new MyPlayerClass (name);
        api.addPlayer<MyQueueClass, MyPlayerClass> (queues[queue], player);
    }

    public void updatePlayer (){
        MyPlayerClass p = players[player];
        p.name = "josefina12354";
        api.updatePlayer<MyQueueClass, MyPlayerClass>(queues[queue], p);
    }

    public void updateQueue(){
        MyQueueClass q = queues[queue];
        q.name = "joaquina123456";
        api.updateQueue<MyQueueClass>(q);
    }

}