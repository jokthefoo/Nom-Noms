using UnityEngine;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour {

    public Sprite[] sprites;
    public bool fridge = false;
    public GameObject fridgeItem;
    public MainGame gameInstance;

    private string foodName = "";
    private string desc = "";

    private int stamina, mood, strength, magic, hp;

    private bool anim = false;
    private float startTime;
    private string _barcode = "";

    private Vector3 startPos;
    private Vector3 endPos;
    private Noobles noobles;
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
                    desc += "Orange";
                    stamina = 0;
                    mood = 1;
                    strength = 0;
                    magic = 3;
                    hp = 100;
                    break;
                case "012345678974":
                    foodName = "Pizza";
                    desc += "Pizza";
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

            if (!fridge)
            {
                GameObject item = Instantiate(gameInstance.foodItemPrefab, gameInstance.otherScrollContent.transform);
                item.GetComponent<FoodItem>().fridge = true;
                item.GetComponent<FoodItem>().barcode = barcode;
                item.GetComponent<FoodItem>().gameInstance = gameInstance;

                fridgeItem = item;
            }
        }
    }
	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(() => clickedOn());
        startPos = GameObject.Find("DropPos").transform.position;
        endPos = GameObject.Find("Noobles").transform.position;
        noobles = GameObject.Find("Noobles").GetComponent<Noobles>();
    }
	
	// Update is called once per frame
	void Update () {
        
		if(anim)
        {
            float dist = (Time.time - startTime) * 10f;
            float lerpVal = dist / Vector3.Distance(startPos, Vector3.zero);
            transform.position = Vector3.Lerp(startPos, endPos, lerpVal);
        }
        if(transform.position == endPos)
        {
            anim = false;
            Destroy(this.gameObject);
        }
    }
       
    public void clickedOn()
    {
        if(fridge)
        {
            gameInstance.foodInfoCanvas.GetComponentInChildren<Image>().sprite = GetComponent<Image>().sprite;
            gameInstance.foodInfoCanvas.GetComponentInChildren<Text>().text = desc;
            gameInstance.foodInfoCanvas.GetComponent<CanvasGroup>().alpha = 1;
            gameInstance.foodInfoCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;

            int tempHp = hp;
            if(hp < 0)
            {
                gameInstance.foodStatsBars[0].GetComponent<Image>().color = Color.gray;
                tempHp *= -1;
            }
            else
            {
                gameInstance.foodStatsBars[0].GetComponent<Image>().color = Color.red;
            }
            gameInstance.foodStatsBars[0].GetComponent<Image>().fillAmount = (tempHp / 500f);
            gameInstance.foodStatsBars[1].GetComponent<Image>().fillAmount = (magic / 10f);
            gameInstance.foodStatsBars[2].GetComponent<Image>().fillAmount = (mood / 10f);
            gameInstance.foodStatsBars[3].GetComponent<Image>().fillAmount = (strength / 10f);
            gameInstance.foodStatsBars[4].GetComponent<Image>().fillAmount = (stamina / 10f);
        }
        else
        {
            // Show stats
            noobles.nbHP += hp;
            noobles.nbMagic += magic;
            noobles.nbMood += mood;
            noobles.nbStamina += stamina;
            noobles.nbStrength += strength;

            if(noobles.nbHP < 0)
            {
                noobles.nbHP = 0;
            }
            else if(noobles.nbHP > 1000)
            {
                noobles.nbHP = 1000;
            }

            int sum = noobles.nbMagic + noobles.nbMood + noobles.nbStamina + noobles.nbStrength;
            int sub = (sum - 20) / 4;

            noobles.nbMagic -= sub;
            noobles.nbMood -= sub;
            noobles.nbStamina -= sub;
            noobles.nbStrength -= sub;

            noobles.nbMagic = Mathf.Clamp(noobles.nbMagic, 0, 20);
            noobles.nbMood = Mathf.Clamp(noobles.nbMood, 0, 20);
            noobles.nbStamina = Mathf.Clamp(noobles.nbStamina, 0, 20);
            noobles.nbStrength = Mathf.Clamp(noobles.nbStrength, 0, 20);
            
            if (noobles.nbMagic > 3 && noobles.nbMood > 3 && noobles.nbStamina > 3 && noobles.nbStrength > 3)
            {
                //Happy
                noobles.showEmotion(0);
            }
            else if (noobles.nbMood < 2)
            {
                //Uneasy
                noobles.showEmotion(1);
            }
            else if (noobles.nbMagic > 7)
            {
                //Content
                noobles.showEmotion(2);
            }
            else if(noobles.nbStamina > 7 && noobles.nbStrength < 3)
            {
                //Unsure
                noobles.showEmotion(3);
            }
            else if (noobles.nbMagic == 0 || noobles.nbStrength == 0 || noobles.nbMood == 0 || noobles.nbStamina == 0)
            {
                //Notgood
                noobles.showEmotion(4);
            }

            //Destroy(this.gameObject);
            anim = true;
            startTime = Time.time;
            transform.SetParent(transform.parent.parent.parent.parent.parent.parent);
            transform.position = startPos;
            noobles.declareChange();

            Destroy(fridgeItem);
        }
    }
}
