using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using AUP;

public class ImagePickerDemo2 : MonoBehaviour
{
    private const string TAG = "[ImagePickerDemo2]: ";

    private ImagePickerPlugin imagePickerPlugin;
    public Text statusText;

    // where the image paths will be stored
    private string[] imagePaths;

    public Texture imagePlaceHolder;
    public RawImage rawImage1;
    public RawImage rawImage2;

    private Dispatcher dispatcher;

    // Use this for initialization
    void Start()
    {
        // needed to run the callback on the main thread
        dispatcher = Dispatcher.GetInstance();

        imagePickerPlugin = ImagePickerPlugin.GetInstance();
        imagePickerPlugin.SetDebug(0);
        // init image picker instance
        imagePickerPlugin.Init();
        // add eventlisteners
        AddEventListener();
    }

    private void AddEventListener()
    {        
        imagePickerPlugin.OnGetImagesComplete += onGetImagesComplete;
        imagePickerPlugin.OnGetImageCancel += onGetImageCancel;
        imagePickerPlugin.OnGetImageFail += onGetImageFail;
    }

    // remove event listeners
    private void RemoveEventListener()
    {        
        imagePickerPlugin.OnGetImagesComplete -= onGetImagesComplete;
        imagePickerPlugin.OnGetImageCancel -= onGetImageCancel;
        imagePickerPlugin.OnGetImageFail -= onGetImageFail;
    }

    private void OnDestroy()
    {
        RemoveEventListener();
    }

    public void GetImages()
    {
        // opens up a custom gallery for you to select images
        imagePickerPlugin.GetImages();
    }

    private void UpdateStatus(string status)
    {
        if (statusText != null)
        {
            statusText.text = String.Format("Status: {0}", status);
        }
    }

    // loads the selected images from your adroid mobile phone
    // this sample is just 2 images but you can add more if you like
    private void DelayLoadImage()
    {
        
        if (imagePaths.Length > 0)
        {
            //loads texture
            if (!imagePaths[0].Equals("", StringComparison.Ordinal))
            {
                rawImage1.texture = AUP.Utils.LoadTexture(imagePaths[0]);
            }
            else
            {
                rawImage1.texture = imagePlaceHolder;
            }
        }
        else
        {
            rawImage1.texture = imagePlaceHolder;
        }

        if (imagePaths.Length > 1)
        {
            
            if (!imagePaths[1].Equals("", StringComparison.Ordinal))
            {
                rawImage2.texture = AUP.Utils.LoadTexture(imagePaths[1]);
            }
            else
            {
                rawImage2.texture = imagePlaceHolder;
            }
        }
        else
        {
            rawImage2.texture = imagePlaceHolder;
        }


        UpdateStatus("load image complete");
    }

    private void LoadImageMessage()
    {
        UpdateStatus("Loading Image...");
    }

    private void onGetImagesComplete(string rawImagePath)
    {
        dispatcher.InvokeAction(
            () =>
            {
                UpdateStatus(TAG + "get Images Complete");
                Debug.Log(TAG + "onGetImagesComplete imagePath " + rawImagePath);

                imagePaths = rawImagePath.Split(',');

                string[] getImagePaths = rawImagePath.Split(',');
                Debug.Log(TAG + " result length " + getImagePaths.Length);

                //sample on checking results paths of images
                /*foreach (string path in getImagePaths)
                {
                    Debug.Log(TAG + " path " + path);
                }*/

                //sample showing the result image path of image1 and image2
                if (imagePaths.Length > 0)
                {
                    string imagePath1 = getImagePaths.GetValue(0).ToString();    
                    Debug.Log(TAG + " imagePath1: " + imagePath1);
                }

                if (imagePaths.Length > 1)
                {

                    string imagePath2 = getImagePaths.GetValue(1).ToString();
                    Debug.Log(TAG + " imagePath2: " + imagePath2);    

                }

                // cancel active invoke
                CancelInvoke("LoadImageMessage");
                CancelInvoke("DelayLoadImage");

                // call a new invoke
                Invoke("LoadImageMessage", 0.3f);
                Invoke("DelayLoadImage", 0.5f);
            }
        );
    }

    private void onGetImageCancel()
    {
        dispatcher.InvokeAction(
            () =>
            {
                UpdateStatus("onGetImageCancel");
            }
        );
    }

    private void onGetImageFail()
    {
        dispatcher.InvokeAction(
            () =>
            {
                UpdateStatus("onGetImageFail");
            }
        );
    }
}

