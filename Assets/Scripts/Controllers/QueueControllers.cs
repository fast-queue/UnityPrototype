using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class QueueControllers : MonoBehaviour {

    float elapsed = 0f;
    Queue[] queues;
    int page = 0;

    //Canvaas
    public GameObject canvas;
    //

    // GUI
    public GameObject playerGUI;
    public GameObject queueGUI;

    // Rooms info
    public GameObject prefab;
    private List<Vector2> listPos = new List<Vector2>();

    private Vector2 size = new Vector2(300, 150);
    //

    public Text message;

    // Create room Input
    public InputField roomInputNameText;

    void Start() {
        queues = new Queue[0];
        listPos.Add(new Vector2(-72, 147));
        listPos.Add(new Vector2(-72, 26));
        listPos.Add(new Vector2(-72, -97));
        listPos.Add(new Vector2(189, 147));
        listPos.Add(new Vector2(189, 26));
        listPos.Add(new Vector2(189, -97));
    }

    public void doLogout(){
        // Play sound on manager
        Manager.Instance.playEnterButtonSFX();

        // set status to lobby
        Manager.Instance.status = (int)Manager.State.MENU;

        Manager.Instance.user = null;
        SceneManager.LoadScene("Login Scene", LoadSceneMode.Single);
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= 1f)
        {
            elapsed = elapsed % 1f;
            if(Manager.Instance.status == (int)Manager.State.LOBBY)
            {
                queueGUI.SetActive(true);
                playerGUI.SetActive(false);
                queues = Manager.Instance.api.getAllQueue<Queue>();
                refreshRooms();
            }
        }
    }

    public void createRoom()
    {
        if(roomInputNameText.text != "" && roomInputNameText != null){
            // The queue is responsible to existing itself
            Queue q = new Queue(roomInputNameText.text, 5);
            // add the queue to the server when it's initilized
            var respQ = Manager.Instance.api.addQueue<Queue>(q); // get the id from the server
            q._id = respQ._id;
            Manager.Instance.queues.Add(respQ._id, respQ);

            //enter the room after creating it
            enterRoom(respQ);
            return;
        }
        message.text = "Insert a name for room";
    }

    public void enterRoom(Queue q)
    {
        if (q.addPlayer(Manager.Instance.user)){
            // delete rooms from UI
            destroyAllRooms();

            // set state to room
            Manager.Instance.status = (int) Manager.State.ROOM;
            GetComponent<PlayersController>().setQueue(q);
            return;
        } 
        message.text = "Max players reached on room";
    }

    public void leaveRoom()
    {
        var pController = GetComponent<PlayersController>();
        // remove player from queue
        pController.getQueue().removePlayer(Manager.Instance.user);
        // set id to null           
        Manager.Instance.user._id = null;
        // set queue to null
        pController.setQueue(null);
        // set status to lobby
        Manager.Instance.status = (int)Manager.State.LOBBY;
    }

    private void refreshRooms()
    {
        Manager.Instance.queues.Clear();

        foreach (Queue q in queues)
        {
            // Delete in case of no players
            // if(q.numPlayers == 0)
            // {
            //     Manager.Instance.api.deleteQueue<Queue>(q);
            //     continue;
            // }
            Manager.Instance.queues.Add(q._id, q);
            Debug.Log(q.toJson());
        }
        destroyAllRooms();
        drawRooms();
    }

    private void drawRooms()
    {
        int n = page * 6;
        for (int i = n; i < queues.Length  && i < (n + 6); i++)
        {
            var r = Instantiate(prefab) as GameObject;
            r.transform.parent = canvas.transform;
            r.transform.localScale = new Vector3(1, 1, 1);
            r.transform.localPosition = new Vector3(listPos[i - n].x, listPos[i - n].y, 5);
            r.tag = "Rooms";
            // set queue on object
            r.GetComponent<QueueObject>().queue = queues[i - n];

            //set room name
            var roomName = r.transform.Find("RoomName").GetComponent<Text>();
            roomName.text = queues[i].name;

            // Set players on room
            var playersOnRoom = r.transform.Find("Text").GetComponent<Text>();
            playersOnRoom.text = (queues[i].numPlayers.ToString() + "/" + queues[i].maxPlayers.ToString());
        }
            
    }

    private void destroyAllRooms()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag("Rooms");

        foreach (GameObject g in gameObjects)
        {
            Destroy(g);
        }
    }

    private void removeQueue(Queue q)
    {
        Manager.Instance.api.deleteQueue<Queue>(q);
        Manager.Instance.queues.Remove(q._id);
    }

    public void addPage(int n)
    {
        page += n;
        if (page < 0)
            page = 0;
        refreshRooms();
        elapsed = 0f;
    }
}
