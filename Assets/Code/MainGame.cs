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

    public GameObject button;
    public GameObject camImage;
    public GameObject scrollContent;
    public GameObject foodItemPrefab;

    IEnumerator Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        yield return new WaitForSeconds(.5f);

        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;
        barcodeReader = new BarcodeReader();

        float height = 2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * 1.3f;
        float width = height * Screen.width / Screen.height;

        camImage.transform.localScale = new Vector3(width, 1, height);
        camImage.GetComponent<Renderer>().material.mainTexture = camTexture;
        if (camTexture != null)
        {
            camTexture.Play();
        }
        cameraInitialized = true;
    }
    private void Update()
    {
        if (Time.time >= nextUpdate && cameraActive)
        {
            nextUpdate = Time.time + 1f;
            decode();
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            GameObject item = Instantiate(foodItemPrefab, scrollContent.transform);
            item.GetComponent<FoodItem>().barcode = "028400275132";
        }else if (Input.GetKeyDown(KeyCode.B))
        {
            GameObject item = Instantiate(foodItemPrefab, scrollContent.transform);
            item.GetComponent<FoodItem>().barcode = "078742233536";
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
                    cameraActive = false;
                    camImage.GetComponent<Renderer>().enabled = false;
                    button.GetComponentInChildren<Text>().text = "Scan Barcode";

                    GameObject item = Instantiate(foodItemPrefab, scrollContent.transform);
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
            cameraActive = false;
            button.GetComponentInChildren<Text>().text = "Scan Barcode";
            camImage.GetComponent<Renderer>().enabled = false;
        }
        else
        {
            cameraActive = true;
            button.GetComponentInChildren<Text>().text = "Stop Scan";
            camImage.GetComponent<Renderer>().enabled = true;
        }
    }
}
