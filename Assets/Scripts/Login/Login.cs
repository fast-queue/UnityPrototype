using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {
    public Dropdown classes;
    public InputField Name;
    public Button login;
    public Text warning;
    AudioSource clickSound;

	// Use this for initialization
	void Start () {
        clickSound = GetComponent<AudioSource>();
    }

    public void doLogin()
    {
        clickSound.Play();
        if(Name.text == "" || Name.text == " ")
        {
            warning.text = "Please, enter a valid name";
            return;
        }
        login.enabled = false;
        Manager.Instance.user = new Player(Name.text, classes.value);
        SceneManager.LoadScene("Rooms");
    }

}
