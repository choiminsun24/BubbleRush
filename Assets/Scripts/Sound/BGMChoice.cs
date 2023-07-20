using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMChoice : MonoBehaviour
{
    public AudioClip lobby;
    public AudioClip inGame;

    AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void changeToInGame()
    {
        gameObject.GetComponent<AudioSource>().clip = inGame;
    }

    public void changeToLobby()
    {
        gameObject.GetComponent<AudioSource>().clip = lobby;
    }
}
