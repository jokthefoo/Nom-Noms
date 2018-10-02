using UnityEngine;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour {

    public Sprite[] sprites;
    public bool fridge = false;
    public GameObject fridgeItem;
    public MainGame gameInstance;

    private string foodName = "";
    private string desc = "i am ";
    private int fatNum;
    private static int nooblesHP = 5;
    private GameObject fat, norm, starve;

    public string barcode
    {
        get { return _barcode; }
        set { _barcode = value;
            Image im = GetComponent<Image>();

            switch (barcode)
            {
                case "012345678905":
                    foodName = "Apple";
                    desc += foodName;
                    fatNum = -2;
                    break;
                case "012345678912":
                    foodName = "Burger";
                    desc += foodName;
                    fatNum = 2;
                    break;
                case "012345678929":
                    foodName = "Corn Dog";
                    desc += "Dog, Corn Dog.";
                    fatNum = 2;
                    break;
                case "012345678936":
                    foodName = "Donut";
                    desc += "Donut";
                    fatNum = 2;
                    break;
                case "012345678943":
                    foodName = "Egg";
                    desc += "Egg, i came first";
                    fatNum = -4;
                    break;
                case "012345678950":
                    foodName = "OJJ";
                    desc += "Orange Juice";
                    fatNum = -2;
                    break;
                case "012345678967":
                    foodName = "Orange";
                    desc += "Orange, rhymes with Orange";
                    fatNum = -2;
                    break;
                case "012345678974":
                    foodName = "Pizza";
                    desc += "Pizza, America's favorite vegetable";
                    fatNum = 2;
                    break;
                case "012345678981":
                    foodName = "Ramen";
                    desc += foodName;
                    fatNum = 2;
                    break;
                case "012345678998":
                    foodName = "Soda";
                    desc += foodName;
                    fatNum = 2;
                    break;
                case "000000000017":
                    foodName = "Water";
                    desc += foodName;
                    fatNum = -2;
                    break;
                default:
                    foodName = "Bacon";
                    desc += foodName;
                    fatNum = 4;
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

        fat = GameObject.Find("Fat");
        norm = GameObject.Find("Normal");
        starve = GameObject.Find("Starving");

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
		
	}

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
            nooblesHP += fatNum;

            if (nooblesHP > 10)
            {
                fat.GetComponent<Renderer>().enabled = true;
                norm.GetComponent<Renderer>().enabled = false;
                starve.GetComponent<Renderer>().enabled = false;
            }
            else if(nooblesHP < 0)
            {
                fat.GetComponent<Renderer>().enabled = false;
                norm.GetComponent<Renderer>().enabled = false;
                starve.GetComponent<Renderer>().enabled = true;
            }
            else
            {
                fat.GetComponent<Renderer>().enabled = false;
                norm.GetComponent<Renderer>().enabled = true;
                starve.GetComponent<Renderer>().enabled = false;
            }

            if(nooblesHP > 15)
            {
                nooblesHP = 15;
            }
            else if(nooblesHP < -5)
            {
                nooblesHP = -5;
            }

            Destroy(this.gameObject);
            Destroy(fridgeItem);
        }
    }
}
