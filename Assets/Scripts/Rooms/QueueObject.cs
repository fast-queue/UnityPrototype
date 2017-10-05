using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueObject : MonoBehaviour {
    public Queue queue;

    public void enterRoom()
    {
        GameObject.Find("Rooms").GetComponent<QueueControllers>().enterRoom(queue);
    }
    
}
