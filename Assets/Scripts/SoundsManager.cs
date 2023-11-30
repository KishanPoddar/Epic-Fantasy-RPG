using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;
    private AudioSource soundSource;
    public static int soundsIndex = 0;
    private int checkIndex = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        soundSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        soundSource.clip = sounds[0];
        soundSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(soundsIndex > checkIndex)
        {
            soundSource.clip = sounds[soundsIndex];
            soundSource.Play();
            checkIndex++;
        }
    }
}
