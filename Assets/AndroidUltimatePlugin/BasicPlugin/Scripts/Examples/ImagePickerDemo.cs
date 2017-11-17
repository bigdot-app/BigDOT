using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using AUP;

public class ImagePickerDemo : MonoBehaviour
{
    private const string TAG = "[ImagePickerDemo]: ";

    private SharePlugin sharePlugin;
    private ImagePickerPlugin imagePickerPlugin;

    public Text statusText;
    private string imagePath = "";

    public RawImage rawImage;
    public Button shareButton;

    private Dispatcher dispatcher;

    // Use this for initialization
    void Start()
    {
        dispatcher = Dispatcher.GetInstance();

        sharePlugin = SharePlugin.GetInstance();
        sharePlugin.SetDebug(0);

        imagePickerPlugin = ImagePickerPlugin.GetInstance();
        imagePickerPlugin.SetDebug(0);
        // init image picker instance
        imagePickerPlugin.Init();
        // add eventlisteners
        AddEventListener();

        EnableDisableShareButton(false);
    }

    private void AddEventListener()
    {
        imagePickerPlugin.OnGetImageComplete += onGetImageComplete;
        imagePickerPlugin.OnGetImageCancel += onGetImageCancel;
        imagePickerPlugin.OnGetImageFail += onGetImageFail;
    }

    // remove event listeners
    private void RemoveEventListener()
    {
        imagePickerPlugin.OnGetImageComplete -= onGetImageComplete;
        imagePickerPlugin.OnGetImageCancel -= onGetImageCancel;
        imagePickerPlugin.OnGetImageFail -= onGetImageFail;
    }

    private void OnDestroy()
    {
        RemoveEventListener();
    }

    public void GetImage()
    {
        imagePickerPlugin.GetImage();
        EnableDisableShareButton(false);
    }

    public void ShareImage()
    {
        if (!imagePath.Equals("", StringComparison.Ordinal))
        {
            sharePlugin.ShareImage("MyPictureSubject", "MyPictureSubjectContent", imagePath);
            UpdateStatus("Sharing Picture");
        }
        else
        {
            Debug.Log("[CameraDemo] imagepath is empty");
            UpdateStatus("can't image path is empty");
        }
    }

    private void UpdateStatus(string status)
    {
        if (statusText != null)
        {
            statusText.text = String.Format("Status: {0}", status);
        }
    }

    private void DelayLoadImage()
    {
        //loads texture
        rawImage.texture = AUP.Utils.LoadTexture(imagePath);
		
        UpdateStatus("load image complete");
        EnableDisableShareButton(true);
    }

    private void EnableDisableShareButton(bool val)
    {
        shareButton.interactable = val;
    }

    private void LoadImageMessage()
    {
        UpdateStatus("Loading Image...");
    }

    private void onGetImageComplete(string imagePath)
    {
        dispatcher.InvokeAction(
            () =>
            {
                this.imagePath = imagePath;

                UpdateStatus("Get Image Complete");
				
                Invoke("LoadImageMessage", 0.3f);
                Invoke("DelayLoadImage", 0.5f);

                Debug.Log(TAG + "onGetImageComplete imagePath " + imagePath);
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
