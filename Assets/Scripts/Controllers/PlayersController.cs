using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayersController : MonoBehaviour {

    private float elapsed = 0f;
    // prefab
    public GameObject roomPrefab;
    //canvas
    public Canvas canvas;

    //GUI
    public GameObject playerGUI;
    public GameObject queueGUI;

    // Player's info
    public Text Name;
    public Text Class;

    Player[] players;

    //Queue he's in
    private Queue queue;
    
    private List<Vector2> listPos = new List<Vector2>();

    // Use this for initialization
    void Start () {
        Name.text = Manager.Instance.user.name;
        Class.text = Manager.Instance.user.getClassText();
        listPos.Add(new Vector2(-72, 147));
        listPos.Add(new Vector2(-72, 26));
        listPos.Add(new Vector2(-72, -97));
        listPos.Add(new Vector2(189, 147));
        listPos.Add(new Vector2(189, 26));
        listPos.Add(new Vector2(189, -97));
    }
	
	// Update is called once per frame
	void Update () {
        elapsed += Time.deltaTime;
        if (elapsed >= 1f)
        {
            elapsed = elapsed % 1f;
            if(Manager.Instance.status == (int) Manager.State.ROOM)
            {
                playerGUI.SetActive(true);
                queueGUI.SetActive(false);
                refreshPlayers();
                drawPlayers();
            }
        }
    }

    void refreshPlayers()
    {
        Manager.Instance.Players.Clear();

        players = Manager.Instance.api.getPlayers<Queue, Player>(queue);

        foreach (Player p in players)
        {
            Manager.Instance.Players.Add(p._id, p);
        }
    }

    void drawPlayers()
    {
        // clear the scenario for new players
        destroyAllPlayers();

        for (int i = 0; i < players.Length; i++)
        {
            var r = Instantiate(roomPrefab) as GameObject;
            r.transform.parent = canvas.transform;
            r.transform.localScale = new Vector3(1, 1, 1);
            r.transform.localPosition = new Vector3(listPos[i].x, listPos[i].y, 5);
            r.tag = "Players";

            // setPlayerName
            var roomName = r.transform.Find("RoomName").GetComponent<Text>();
            roomName.text = players[i].name;
        }
    }

    private void destroyAllPlayers()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag("Players");

        foreach (GameObject g in gameObjects)
        {
            Destroy(g);
        }
    }

    public void setQueue(Queue q)
    {
        if(q == null)
        {
            destroyAllPlayers();
        }
        this.queue = q;
    }

    public Queue getQueue()
    {
        return this.queue;
    }

    void OnDestroy()
    {
        if(queue != null)
        {
            Manager.Instance.api.deletePlayer<Queue, Player>(queue, Manager.Instance.user);
        }
    }
}
