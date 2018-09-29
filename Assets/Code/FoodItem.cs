using UnityEngine;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour {

    public Sprite[] sprites;
    public bool fridge = false;
    public GameObject fridgeItem;
    public MainGame gameInstance;

    private string foodName = "";
    private string desc = "i am ";

    public string barcode
    {
        get { return _barcode; }
        set { _barcode = value;
            Image im = GetComponent<Image>();

            switch (barcode)
            {
                case "028400275132":
                    foodName = "bacon";
                    desc += foodName;
                    break;
                case "078742233536":
                    foodName = "egg";
                    desc += foodName;
                    break;
                default:
                    foodName = "bacon";
                    desc += foodName;
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

        while(gameInstance == null) { }
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
            GameObject noobles = GameObject.Find("Sphere");
            switch (foodName)
            {
                case "bacon":
                    noobles.GetComponent<MeshRenderer>().material.color = Color.red;
                    noobles.GetComponent<Transform>().localScale *= 1.3f;
                    break;
                case "egg":
                    noobles.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    noobles.GetComponent<Transform>().localScale *= .8f;
                    break;
                default:
                    break;
            }
            Destroy(this.gameObject);
            Destroy(fridgeItem);
        }
    }
}
