using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace AUP{
	public class Utils{
		public static void Message(string tag, string message){
			Debug.LogWarning(tag + message);
		}
		
		//take screen shot then share via intent
		public static IEnumerator TakeScreenshot(string screenShotPath,string screenShotName){
			yield return new WaitForEndOfFrame();
			
			int width = Screen.width;
			int height = Screen.height;
			Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
			
			// Read screen contents into the texture
			tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
			
			tex.Apply();

			byte[] screenshot;

			string[] pathParts = screenShotName.Split('.');
			
			if(pathParts.Length > 1){
				string format = pathParts.GetValue(1).ToString();
				Debug.Log( " format " + format );
				if(format.Equals("png",StringComparison.Ordinal)){
					screenshot = tex.EncodeToPNG();
					Debug.Log( "screen shot set to png format");
				}else if(format.Equals("jpg",StringComparison.Ordinal)){
					screenshot = tex.EncodeToJPG();
					Debug.Log( "screen shot set to jpg format");
				}else{
					screenshot = tex.EncodeToJPG();
					Debug.Log( "screen shot unknown format default to jpg");
				}
			}else{
				screenshot = tex.EncodeToJPG();
				Debug.Log( "screen shot no format default to jpg");
			}

			//saving to phone storage
			System.IO.File.WriteAllBytes(screenShotPath,screenshot);
		}

		public static IEnumerator TakeScreenshotNoSave(Action<Texture2D>OnComplete){
			yield return new WaitForEndOfFrame();

			int width = Screen.width;
			int height = Screen.height;
			Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

			// Read screen contents into the texture
			tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);

			tex.Apply();

			if(null!=OnComplete){
				OnComplete(tex);
			}
		}
		
		public static IEnumerator SaveTexureOnDevice(string texturePath, Texture2D texture){
			yield return new WaitForEndOfFrame();
			
			Color32[] pix = texture.GetPixels32();
			//System.Array.Reverse(pix);
			Texture2D destTex = new Texture2D(texture.width, texture.height);
			destTex.SetPixels32(pix);
			destTex.Apply();
			
			//saving to phone storage
			byte[] existingTexture = destTex.EncodeToPNG();
			System.IO.File.WriteAllBytes(texturePath,existingTexture);
		}
		
		public static Texture2D LoadTexture(string imagePath){
			Texture2D tex = new Texture2D(1, 1);
			
			if (System.IO.File.Exists(imagePath)){
				byte[] bytes = System.IO.File.ReadAllBytes(imagePath);
				tex.LoadImage(bytes);
			}
			
			return tex;
		}
		
		public static IEnumerator LoadTextureFromWeb(string url, Action <Texture2D>OnLoadComplete, Action OnLoadFail){
			// Start a download of the given URL
			WWW www = new WWW(url);
			
			// Wait for download to complete
			yield return www;
			
			if(www.texture != null){
				Texture2D texture = www.texture as Texture2D;
				OnLoadComplete(texture);
			}else{
				OnLoadFail();
			}
		}

		public static T DeepCopy<T>(T obj)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, obj);
				stream.Position = 0;
				
				return (T)formatter.Deserialize(stream);
			}
		}
	}
}