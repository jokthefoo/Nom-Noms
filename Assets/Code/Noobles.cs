using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Noobles : MonoBehaviour {

    public int nbHP = 500;
    public int nbStamina = 5;
    public int nbMood = 5;
    public int nbStrength = 5;
    public int nbMagic = 5;

    public GameObject[] emotions;
    public GameObject[] bars;

    private float startTime;
    private float emoteLength = 1.7f;
    private int curEmote = 0;
    private bool emoting = false;
    private bool wait = false;
    private bool change = false;

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
                if (Time.time > startTime + 1.2f)
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

        if(change)
        {
            change = false;
            bars[0].GetComponent<Image>().fillAmount = (nbHP / 1000f);
            bars[1].GetComponent<Image>().fillAmount = (nbMagic / 20f);
            bars[2].GetComponent<Image>().fillAmount = (nbMood / 20f);
            bars[3].GetComponent<Image>().fillAmount = (nbStrength / 20f);
            bars[4].GetComponent<Image>().fillAmount = (nbStamina / 20f);
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

    public void declareChange()
    {
        change = true;
    }
}
