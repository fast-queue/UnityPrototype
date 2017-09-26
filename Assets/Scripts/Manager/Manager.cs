using UnityEngine;
using System.Collections.Generic;


public class Manager : Singleton<Manager>
{
    protected Manager() { } // guarantee this will be always a singleton only - can't use the constructor!
    public AudioSource Audio;
    public FQ.RestApi api;
    public Dictionary<string, Queue> queues = new Dictionary<string, Queue>();
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

        Audio = GetComponent<AudioSource>();
        Audio.loop = true;
        Audio.Play();
    }

    public Player user;
}