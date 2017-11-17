using UnityEngine;
using System.Collections;
using System;

public class ImagePickerPlugin : MonoBehaviour
{
	
    private static ImagePickerPlugin instance;
    private static GameObject container;
    private const string TAG = "[ImagePickerPlugin]: ";
    private static AUPHolder aupHolder;

    private Action <string> GetImageComplete;

    public event Action <string>OnGetImageComplete
    {
        add{ GetImageComplete += value;}
        remove{ GetImageComplete += value;}
    }

    private Action <string> GetImagesComplete;

    public event Action <string>OnGetImagesComplete
    {
        add{ GetImagesComplete += value;}
        remove{ GetImagesComplete += value;}
    }

    private Action GetImageCancel;

    public event Action OnGetImageCancel
    {
        add{ GetImageCancel += value;}
        remove{ GetImageCancel += value;}
    }

    private Action GetImageFail;

    public event Action OnGetImageFail
    {
        add{ GetImageFail += value;}
        remove{ GetImageFail += value;}
    }
	
    #if UNITY_ANDROID
    private static AndroidJavaObject jo;
    #endif
	
    public bool isDebug = true;
    private bool isInit = false;

    public static ImagePickerPlugin GetInstance()
    {
        if (instance == null)
        {
            container = new GameObject();
            container.name = "ImagePickerPlugin";
            instance = container.AddComponent(typeof(ImagePickerPlugin)) as ImagePickerPlugin;
            DontDestroyOnLoad(instance.gameObject);
            aupHolder = AUPHolder.GetInstance();
            instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
        }
		
        return instance;
    }

    private void Awake()
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.image.ImagePlugin");
        }
        #endif
    }

    /// <summary>
    /// Sets the debug.
    /// 0 - false, 1 - true
    /// </summary>
    /// <param name="debug">Debug.</param>
    public void SetDebug(int debug)
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("SetDebug", debug);
            AUP.Utils.Message(TAG, "SetDebug");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

	
    /// <summary>
    /// initialize the Image Picker Plugin
    /// </summary>
    public void Init()
    {
        if (isInit)
        {
            return;
        }
		
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("init");
            isInit = true;
            SetImagePickerCallbackListener(onGetImageComplete, onGetImagesComplete, onGetImageCancel, onGetImageFail);
            AUP.Utils.Message(TAG, "init");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    /// <summary>
    /// Sets the image picker callback listener.
    /// </summary>
    /// <param name="onGetImageComplete">On get image complete.</param>
    /// <param name="onGetImagesComplete">On get images complete.</param>
    /// <param name="onGetImageCancel">On get image cancel.</param>
    /// <param name="onGetImageFail">On get image fail.</param>
    private void SetImagePickerCallbackListener(Action <string>onGetImageComplete, Action <string>onGetImagesComplete, Action onGetImageCancel, Action onGetImageFail)
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            ImagePickerCallback imagePickerCallback = new ImagePickerCallback();
            imagePickerCallback.onGetImageComplete = onGetImageComplete;
            imagePickerCallback.onGetImagesComplete = onGetImagesComplete;
            imagePickerCallback.onGetImageCancel = onGetImageCancel;
            imagePickerCallback.onGetImageFail = onGetImageFail;
			
            jo.CallStatic("setImagePickerCallbackListener", imagePickerCallback);
            AUP.Utils.Message(TAG, "setCameraCallbackListener");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    /// <summary>
    /// Gets the image.
    /// start activity to pick one image only
    /// </summary>
    public void GetImage()
    {		
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("getImage");
            AUP.Utils.Message(TAG, "getImage");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    /// <summary>
    /// opens up a custom gallery activity for you to select 2 or more images
    /// note: the order of images loaded is depends on how they are organized on your phone
    /// directory and it's not based on the order you select them
    /// </summary>
    public void GetImages()
    {     
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("getImages");
            AUP.Utils.Message(TAG, "getImages");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    /// <summary>
    /// dispatch when image successfully get.
    /// </summary>
    /// <param name="imagePath">Image path.</param>
    private void onGetImageComplete(string imagePath)
    {
        if (null != GetImageComplete)
        {
            GetImageComplete(imagePath);
        }
    }

    /// <summary>
    /// dispatch when images successfully get.
    /// </summary>
    /// <param name="imagePath">Image path.</param>
    private void onGetImagesComplete(string imagePath)
    {
        if (null != GetImagesComplete)
        {
            GetImagesComplete(imagePath);
        }
    }

    /// <summary>
    /// dispatch when user didn't select anything
    /// </summary>
    private void onGetImageCancel()
    {
        if (null != GetImageCancel)
        {
            GetImageCancel();
        }
    }

    /// <summary>
    /// dispatch when fail getting image
    /// </summary>
    private void onGetImageFail()
    {
        if (null != GetImageFail)
        {
            GetImageFail();
        }
    }
}

