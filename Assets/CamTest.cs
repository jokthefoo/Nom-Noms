using System;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class CamTest : MonoBehaviour {

    private WebCamTexture camTexture;
    private Rect screenRect;
    private IBarcodeReader barcodeReader;
    private bool cameraInitialized;
    private bool isDecoding = false;
    private float nextUpdate = 1;
    private bool cameraActive = false;
    public Text displayText;

    void Start()
    {
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;
        barcodeReader = new BarcodeReader();
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
            nextUpdate = Time.time + .1f;
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
                    Debug.Log("DECODED TEXT FROM QR: " + result.Text);
                    ResultPoint[] point = result.ResultPoints;
                    Debug.Log("X: " + point[0].X + " Y: " + point[1].Y);
                    displayText.text = result.Text;
                    cameraActive = false;
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

    void OnGUI()
    {
        // drawing the camera on screen
        if(cameraActive)
        {
            GUI.DrawTexture(screenRect, camTexture, ScaleMode.ScaleToFit);
        }
    }

    public void buttonClick()
    {
        cameraActive = true;
    }
}
