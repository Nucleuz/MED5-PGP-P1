using UnityEngine;
using System.Collections;

public class screenshot : MonoBehaviour {
	private bool takeHiResShot = false;
	public static string ScreenShotName(int width, int height) {
	    return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png", 
	                        Application.dataPath, 
	                        width, height, 
	                        System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
	}
	public void TakeHiResShot() {
	    takeHiResShot = true;
	}
	void LateUpdate() {
	    takeHiResShot |= Input.GetKeyDown("k");
	    if (takeHiResShot) {
	        RenderTexture rt = new RenderTexture(512, 512, 24);
	        GetComponent<Camera>().targetTexture = rt;
	        Texture2D screenShot = new Texture2D(512, 512, TextureFormat.RGB24, false);
	        GetComponent<Camera>().Render();
	        RenderTexture.active = rt;
	        screenShot.ReadPixels(new Rect(0, 0, 512, 512), 0, 0);
	        GetComponent<Camera>().targetTexture = null;
	        RenderTexture.active = null; // JC: added to avoid errors
	        Destroy(rt);
	        byte[] bytes = screenShot.EncodeToPNG();
	        string filename = ScreenShotName(512, 512);
	        System.IO.File.WriteAllBytes(filename, bytes);
	        Debug.Log(string.Format("Took screenshot to: {0}", filename));
	        takeHiResShot = false;
	    }
	}
}
