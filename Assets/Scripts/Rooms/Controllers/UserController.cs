using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UserController : MonoBehaviour {

    public Text Name;
    public Text Class;

    // Use this for initialization
    void Start () {
        Name.text = Manager.Instance.user.name;
        Class.text = Manager.Instance.user.getClassText();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
