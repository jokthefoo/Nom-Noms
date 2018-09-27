using UnityEngine;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour {

    public Sprite[] sprites;
    private string foodName = "";

    public string barcode
    {
        get { return _barcode; }
        set { _barcode = value;
            Image im = GetComponent<Image>();

            switch (barcode)
            {
                case "078742233536":
                    foodName = "bacon";
                    break;
                case "028400275132":
                    foodName = "egg";
                    break;
                default:
                    foodName = "bacon";
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
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void clickedOn()
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
    }
}
