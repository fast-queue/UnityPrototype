using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {
    public Dropdown classes;
    public InputField Name;
    public Button login;
    public Text warning;

    public void doLogin()
    {
        // Play sound on manager
        Manager.Instance.playEnterButtonSFX();

        if(Name.text == "" || Name.text == " ")
        {
            warning.text = "Please, enter a valid name";
            return;
        }
        // set status to lobby
        Manager.Instance.status = (int)Manager.State.LOBBY;

        login.enabled = false;
        Manager.Instance.user = new Player(Name.text, classes.value);
        SceneManager.LoadScene("Rooms");
    }

}
