using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CamCapture : MonoBehaviour
{
    //how many captures so far taken
    public int FileCounter = 0;

    //this flag must be asserted for Capture() to be called
    private bool ready = false;

    //usually user inputs are handled in lateupdate method.
    private void LateUpdate()
    {
        //captures when pressing F9!
        if (Input.GetKeyDown(KeyCode.F9))
        {
            if(ready) Capture();
        }
    }

    //set camera ready
    public void setReady()
    {
        Debug.Log("cam ready for capture");
        ready = true;
    }

    //deassert ready
    public void resetReady()
    {
        Debug.Log("cam will not capture");
        ready = false;
    }


    void Capture()
    {
        Camera MirrorCam = GetComponent<Camera>();

        //get the texture to render camera to (mirror preview)
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = MirrorCam.targetTexture;

        //render the contents of the camera onto the texture
        MirrorCam.Render();

        //make an image from the cam's target texture
        Texture2D Image = new Texture2D(MirrorCam.targetTexture.width, MirrorCam.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, MirrorCam.targetTexture.width, MirrorCam.targetTexture.height), 0, 0);
        Image.Apply();
        RenderTexture.active = currentRT;
        //PNG encoding
        var Bytes = Image.EncodeToPNG();
        Destroy(Image);
        //write to a file
        //!! saved under the project's root / Photos / FileCounter.png!!
        File.WriteAllBytes(Application.dataPath + "/Photos/" + FileCounter + ".png", Bytes);
        FileCounter++;
    }
}
