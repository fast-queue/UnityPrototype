using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueObject : MonoBehaviour {
    public Queue queue;

    public void enterRoom()
    {
        GameObject.Find("Roooms").GetComponent<QueueControllers>().enterRoom(queue);
    }
    
}
