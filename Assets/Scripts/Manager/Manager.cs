using UnityEngine;

public class Manager : Singleton<Manager>
{
    protected Manager() { } // guarantee this will be always a singleton only - can't use the constructor!
    public AudioSource Audio;

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

        Audio = GetComponent<AudioSource>();
        Audio.loop = true;
        Audio.Play();
    }

    public User user;
}