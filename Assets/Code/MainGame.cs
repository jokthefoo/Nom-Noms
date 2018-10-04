using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class MainGame : MonoBehaviour {

    private WebCamTexture camTexture;
    private IBarcodeReader barcodeReader;
    private bool cameraInitialized;
    private bool isDecoding = false;
    private float nextUpdate = 1;
    private bool cameraActive = false;
    /*private Vector3 startPos;
    private Quaternion startRot;
    private Vector3 endPos;
    private Quaternion endRot;
    private float startTime;*/
    //private bool lerping = false;

    public GameObject scanButton;
    public GameObject camImage;
    public GameObject scrollContent;
    public GameObject foodItemPrefab;
    public GameObject otherScrollContent;
    public GameObject foodInfoCanvas;
    public GameObject fridgeCanvas;
    public GameObject camButton;

    IEnumerator Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        yield return new WaitForSeconds(.5f);

        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;
        barcodeReader = new BarcodeReader();

        float height = 2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * 1.2f;
        float width = height * Screen.width / Screen.height;

        camImage.transform.localScale = new Vector3(width, 1, height);
        camImage.GetComponent<Renderer>().material.mainTexture = camTexture;
        cameraInitialized = true;
    }
    private void Update()
    {
        if (Time.time >= nextUpdate && cameraActive)
        {
            nextUpdate = Time.time + 1f;
            decode();
        }
        /*
        if (lerping)
        {
            float dist = (Time.time - startTime) * 65f;
            float lerpVal = dist / Vector3.Distance(startPos, endPos);
            transform.position = Vector3.Lerp(startPos, endPos, lerpVal);
            transform.localRotation = Quaternion.Lerp(startRot, endRot, lerpVal);
        }
        if (transform.position == endPos && transform.rotation == endRot)
        {
            lerping = false;
        }*/

        if(Input.GetKeyDown(KeyCode.Y))
        {
            GameObject item = Instantiate(foodItemPrefab, scrollContent.transform);
            item.GetComponent<FoodItem>().gameInstance = this;
            item.GetComponent<FoodItem>().barcode = "028400275132";
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            GameObject item = Instantiate(foodItemPrefab, scrollContent.transform);
            item.GetComponent<FoodItem>().gameInstance = this;
            item.GetComponent<FoodItem>().barcode = "000000000017";
        }else if(Input.GetKeyDown(KeyCode.Q))
        {

            GameObject item = Instantiate(foodItemPrefab, scrollContent.transform);
            item.GetComponent<FoodItem>().gameInstance = this;
            item.GetComponent<FoodItem>().barcode = "012345678967";
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            GameObject item = Instantiate(foodItemPrefab, scrollContent.transform);
            item.GetComponent<FoodItem>().gameInstance = this;
            item.GetComponent<FoodItem>().barcode = "012345678974";
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject item = Instantiate(foodItemPrefab, scrollContent.transform);
            item.GetComponent<FoodItem>().gameInstance = this;
            item.GetComponent<FoodItem>().barcode = "012345678981";
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject item = Instantiate(foodItemPrefab, scrollContent.transform);
            item.GetComponent<FoodItem>().gameInstance = this;
            item.GetComponent<FoodItem>().barcode = "012345678998";
        }
    }

    private void decode()
    {
        if (cameraInitialized && !isDecoding)
        {
            try
            {
                isDecoding = true;

                // Decode the current frame
                var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
                if (result != null)
                {
                    cameraDeactivate();

                    GameObject item = Instantiate(foodItemPrefab, scrollContent.transform);
                    item.GetComponent<FoodItem>().gameInstance = this;
                    item.GetComponent<FoodItem>().barcode = result.Text;
                }
                else
                {
                    //Debug.Log("Nothing detected.");
                }
                isDecoding = false;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

    public void buttonClick()
    {
        if(cameraActive)
        {
            cameraDeactivate();
        }
        else
        {
            if (camTexture != null)
            {
                camTexture.Play();
            }
            cameraActive = true;
            hideShowInventoryCanvas(true, true);
            hideShowFridgeCanvas(true, true);
            scanButton.GetComponentInChildren<Text>().text = "Stop";
            camImage.GetComponent<Renderer>().enabled = true;
        }
    }

    private void cameraDeactivate()
    {
        if (camTexture != null)
        {
            camTexture.Stop();
        }
        cameraActive = false;
        hideShowInventoryCanvas(true, false);
        hideShowFridgeCanvas(true, false);
        scanButton.GetComponentInChildren<Text>().text = "Scan Barcode";
        camImage.GetComponent<Renderer>().enabled = false;
    }

    public void moveCamera()
    {
        if(transform.position.x == 0)
        {
            camButton.GetComponentInChildren<Text>().text = "Close Fridge";
            transform.position = new Vector3(30, 0, 0);
            hideShowInventoryCanvas(false, true);
        }
        else
        {
            hideShowInventoryCanvas(false, false);
            hideShowFridgeCanvas(false, true);
            camButton.GetComponentInChildren<Text>().text = "Open Fridge";
            transform.position = new Vector3(0, 0, 0);
        }

        /* old
        startPos = transform.position;
        startRot = transform.rotation;

        Transform cam1 = GameObject.Find("CamPos1").transform;
        Transform cam2 = GameObject.Find("CamPos2").transform;
        Transform other;

        if(startPos == cam1.position)
        {
            other = cam2;
            hideShowInventoryCanvas(false, true);
        }
        else
        {
            hideShowInventoryCanvas(false, false);
            hideShowFridgeCanvas(false, true);
            other = cam1;
        }

        endPos = other.position;
        endRot = other.rotation;

        lerping = true;
        startTime = Time.time;
        */
    }

    private void hideShowInventoryCanvas(bool hideButton, bool hide)
    {
        CanvasGroup grp;
        if (hideButton)
        {
            grp = scrollContent.transform.parent.parent.parent.parent.GetComponent<CanvasGroup>();
        }
        else
        {
            grp = scrollContent.transform.parent.parent.parent.GetComponent<CanvasGroup>();
        }

        if(hide)
        {
            grp.alpha = 0;
            grp.blocksRaycasts = false;
        }
        else
        {
            grp.alpha = 1;
            grp.blocksRaycasts = true;
        }
    }

    private void hideShowFridgeCanvas(bool fridge, bool hide)
    {
        CanvasGroup grp;
        if (fridge)
        {
            grp = fridgeCanvas.GetComponent<CanvasGroup>();
        }
        else
        {
            grp = foodInfoCanvas.GetComponent<CanvasGroup>();
        }

        if(hide)
        {
            grp.alpha = 0;
            grp.blocksRaycasts = false;
        }
        else
        {
            grp.alpha = 1;
            grp.blocksRaycasts = true;
        }
    }
}
