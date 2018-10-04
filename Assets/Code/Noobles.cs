using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noobles : MonoBehaviour {

    public int nbHP = 500;
    public int nbStamina = 5;
    public int nbMood = 5;
    public int nbStrength = 5;
    public int nbMagic = 5;

    public GameObject[] emotions;

    private float startTime;
    private float emoteLength = 1.5f;
    private int curEmote = 0;
    private bool emoting;
    private bool wait;
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(emoting)
        {
            if(wait)
            {
                if (Time.time > startTime + 1.5f)
                {
                    emotions[curEmote].GetComponent<Renderer>().enabled = true;
                    wait = false;
                    startTime = Time.time;
                }
            }
            else
            {
                if (Time.time > startTime + emoteLength)
                {
                    emoting = false;
                    emotions[curEmote].GetComponent<Renderer>().enabled = false;
                }
            }
        }
	}

    public void showEmotion(int emote)
    {
        if(!emoting)
        {
            emoting = true;
            wait = true;
            curEmote = emote;
            startTime = Time.time;
        }
    }
}
