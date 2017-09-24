using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class QueueControllers : MonoBehaviour {

    float elapsed = 0f;
    Queue[] queues;

    // Create room Input
    public Text roomInputNameText;
    //
	
	void Start () {
        queues = new Queue[0];
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= 1f)
        {
            elapsed = elapsed % 1f;
            queues = Manager.Instance.api.getAllQueue<Queue>();
            refreshRooms();
        }
    }

    public void createRoom()
    {
        Queue q = new Queue(roomInputNameText.text, 5);
        Manager.Instance.api.addQueue<Queue>(q);
    }

    private void refreshRooms()
    {

    }
}
