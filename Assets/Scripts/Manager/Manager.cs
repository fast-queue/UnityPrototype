using UnityEngine;
using System.Collections.Generic;


public class Manager : Singleton<Manager>
{
    protected Manager() { } // guarantee this will be always a singleton only - can't use the constructor!
    private AudioSource[] sfx;

    // api
    public FQ.RestApi api;

    // Players and Queues
    public Dictionary<string, Queue> queues = new Dictionary<string, Queue>();
    public Dictionary<string, Player> Players = new Dictionary<string, Player>();

    public Player user;
    public int status;

    void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
        api = new FQ.RestApi("http://fq-api.bovendorp.org", "testkeytoeveryone");

        sfx = GetComponents<AudioSource>();
        sfx[0].loop = true;
        sfx[0].Play();

        status = (int) State.MENU;
    }
    
    public void leaveGame(){
        Application.Quit();
    }

    public void playEnterButtonSFX()
    {
        if(!sfx[1].isPlaying)
            sfx[1].Play();
    }
    public enum State
    {
        MENU = 0,
        LOBBY,
        ROOM
    }
}
