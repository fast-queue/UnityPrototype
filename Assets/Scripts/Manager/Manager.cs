using UnityEngine;

public class Manager : Singleton<Manager>
{
    protected Manager() { } // guarantee this will be always a singleton only - can't use the constructor!
    public AudioSource Audio;
    public FQ.RestApi api;

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