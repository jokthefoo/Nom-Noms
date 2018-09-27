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
    public GameObject displayText;
    public GameObject scrollContent;

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
        displayText.GetComponent<Text>().text = "";
    }
    private void Update()
    {
        if (Time.time >= nextUpdate && cameraActive)
        {
            nextUpdate = Time.time + 1f;
            decode();
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
#if UNITY_EDITOR
                    Debug.Log("DECODED TEXT FROM QR: " + result.Text);
                    ResultPoint[] point = result.ResultPoints;
                    Debug.Log("X: " + point[0].X + " Y: " + point[1].Y);
#endif
                    //displayText.GetComponent<Text>().text = result.Text;

                    cameraActive = false;
                    camImage.GetComponent<Renderer>().enabled = false;
                    button.GetComponentInChildren<Text>().text = "Scan Barcode";
                    String foodString = "";
                    String bacon = "078742233536";
                    String egg = "028400275132";
                    if (bacon == result.Text)
                    {
                        foodString = "Bacon";
                    }
                    else if(egg == result.Text)
                    {
                        foodString = "Egg";
                    }
                    foreach (Image i in scrollContent.GetComponentsInChildren<Image>())
                    {
                        if (i.name == foodString)
                        {
                            i.enabled = true;
                        }
                        else if(i.name == foodString)
                        {
                            i.enabled = true;
                        }
                    }
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
