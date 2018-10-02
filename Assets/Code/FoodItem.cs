using UnityEngine;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour {

    public Sprite[] sprites;
    public bool fridge = false;
    public GameObject fridgeItem;
    public MainGame gameInstance;

    private string foodName = "";
    private string desc = "i am ";
    private int stamina, mood, strength, magic, hp;
    private static int nbHP = 500;
    private static int nbStamina = 5;
    private static int nbMood = 5;
    private static int nbStrength = 5;
    private static int nbMagic = 5;
    //private GameObject fat, norm, starve;

    public string barcode
    {
        get { return _barcode; }
        set { _barcode = value;
            Image im = GetComponent<Image>();

            switch (barcode)
            {
                case "012345678905":
                    foodName = "Apple";
                    stamina = 0;
                    mood = 1;
                    strength = 0;
                    magic = 3;
                    hp = 100;
                    desc += foodName;
                    break;
                case "012345678912":
                    foodName = "Burger";
                    desc += foodName;
                    stamina = 2;
                    mood = 7;
                    strength = 4;
                    magic = 0;
                    hp = -200;
                    break;
                case "012345678929":
                    foodName = "Corn Dog";
                    desc += "Dog, Corn Dog.";
                    stamina = 2;
                    mood = 7;
                    strength = 4;
                    magic = 0;
                    hp = -200;
                    break;
                case "012345678936":
                    foodName = "Donut";
                    desc += "Donut";
                    stamina = 4;
                    mood = 8;
                    strength = 0;
                    magic = 0;
                    hp = -150;
                    break;
                case "012345678943":
                    foodName = "Egg";
                    desc += "egg";
                    stamina = 0;
                    mood = 1;
                    strength = 0;
                    magic = 0;
                    hp = 10;
                    break;
                case "012345678950":
                    foodName = "OJJ";
                    desc += "Orange Juice";
                    stamina = 0;
                    mood = 3;
                    strength = 0;
                    magic = 2;
                    hp = 0;
                    break;
                case "012345678967":
                    foodName = "Orange";
                    desc += "Orange, rhymes with...";
                    stamina = 0;
                    mood = 1;
                    strength = 0;
                    magic = 3;
                    hp = 100;
                    break;
                case "012345678974":
                    foodName = "Pizza";
                    desc += "Pizza, America's favorite vegetable";
                    stamina = 3;
                    mood = 10;
                    strength = 6;
                    magic = 1;
                    hp = -225;
                    break;
                case "012345678981":
                    foodName = "Ramen";
                    desc += foodName;
                    stamina = 3;
                    mood = 2;
                    strength = 1;
                    magic = 0;
                    hp = -25;
                    break;
                case "012345678998":
                    foodName = "Soda";
                    desc += foodName;
                    stamina = 1;
                    mood = 7;
                    strength = 0;
                    magic = 0;
                    hp = -200;
                    break;
                case "000000000017":
                    foodName = "Water";
                    desc += foodName;
                    stamina = 0;
                    mood = 0;
                    strength = 0;
                    magic = 0;
                    hp = 50;
                    break;
                default:
                    foodName = "Bacon";
                    desc += foodName;
                    stamina = 6;
                    mood = 6;
                    strength = 1;
                    magic = 0;
                    hp = -250;
                    break;
            }

            foreach (Sprite s in sprites)
            {
                if (s.name == foodName)
                {
                    im.sprite = s;
                    break;
                }
            }

            im.enabled = true;
        }
    }
    private string _barcode = "";

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(() => clickedOn());

        /*
        fat = GameObject.Find("Fat");
        norm = GameObject.Find("Normal");
        starve = GameObject.Find("Starving");*/

        while (gameInstance == null) { }
        while(barcode == "") { }

        if (!fridge)
        {
            GameObject item = Instantiate(gameInstance.foodItemPrefab, gameInstance.otherScrollContent.transform);
            item.GetComponent<FoodItem>().barcode = barcode;
            item.GetComponent<FoodItem>().fridge = true;
            item.GetComponent<FoodItem>().gameInstance = gameInstance;

            fridgeItem = item;
        }
    }
	
	// Update is called once per frame
	void Update () {
        /*
		if(anim)
        {
            float dist = (Time.time - startTime) * 65f;
            float lerpVal = dist / Vector3.Distance(startPos, endPos);
            transform.position = Vector3.Lerp(startPos, endPos, lerpVal);
            transform.localRotation = Quaternion.Lerp(startRot, endRot, lerpVal);
        }*/
    }

    private bool anim = false;
    private float startTime;

    public void clickedOn()
    {
        if(fridge)
        {
            gameInstance.foodInfoCanvas.GetComponentInChildren<Image>().sprite = GetComponent<Image>().sprite;
            gameInstance.foodInfoCanvas.GetComponentInChildren<Text>().text = desc;
            gameInstance.foodInfoCanvas.GetComponent<CanvasGroup>().alpha = 1;
            gameInstance.foodInfoCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            // Show stats
            nbHP += hp;
            nbMagic += magic;
            nbMood += mood;
            nbStamina += stamina;
            nbStrength += strength;

            if(nbHP < 0)
            {
                nbHP = 0;
            }
            else if(nbHP > 1000)
            {
                nbHP = 1000;
            }

            if(nbHP < 200)
            {
                //starving
            }
            else if(nbHP > 800)
            {
                //thicc
            }

            int sum = nbMagic + nbMood + nbStamina + nbStrength;
            int sub = (sum - 20) / 4;

            nbMagic += sub;
            nbMood += sub;
            nbStamina += sub;
            nbStrength += sub;

            //Destroy(this.gameObject);
            anim = true;
            startTime = Time.time;

            Destroy(fridgeItem);
        }
    }
}
