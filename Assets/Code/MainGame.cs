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
    private Vector3 startPos;
    private Quaternion startRot;
    private Vector3 endPos;
    private Quaternion endRot;
    private float startTime;
    private bool lerping = false;

    public GameObject scanButton;
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

        if (lerping)
        {
            float dist = (Time.time - startTime) * 15f;
            float lerpVal = dist / Vector3.Distance(startPos, endPos);
            transform.position = Vector3.Lerp(startPos, endPos, lerpVal);
            transform.localRotation = Quaternion.Lerp(startRot, endRot, lerpVal);
        }
        if(transform.position == endPos && transform.rotation == endRot)
        {
            lerping = false;
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            GameObject item = Instantiate(foodItemPrefab, scrollContent.transform);
            item.GetComponent<FoodItem>().barcode = "028400275132";
        }else if (Input.GetKeyDown(KeyCode.A))
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
                    scanButton.GetComponentInChildren<Text>().text = "Scan Barcode";

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
            hideCanvas();
            scanButton.GetComponentInChildren<Text>().text = "Scan Barcode";
            camImage.GetComponent<Renderer>().enabled = false;
        }
        else
        {
            cameraActive = true;
            hideCanvas();
            scanButton.GetComponentInChildren<Text>().text = "Stop";
            camImage.GetComponent<Renderer>().enabled = true;
        }
    }
    public void moveCamera()
    {
        startPos = transform.position;
        startRot = transform.rotation;

        Transform cam1 = GameObject.Find("CamPos1").transform;
        Transform cam2 = GameObject.Find("CamPos2").transform;
        Transform other;

        if(startPos == cam1.position)
        {
            other = cam2;
        }
        else
        {
            other = cam1;
        }

        endPos = other.position;
        endRot = other.rotation;

        lerping = true;
        startTime = Time.time;
    }

    private void hideCanvas()
    {
        CanvasGroup grp = scrollContent.transform.parent.parent.parent.GetComponent<CanvasGroup>();

        if(grp.alpha == 1)
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
