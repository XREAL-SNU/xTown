using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CamCapture : MonoBehaviour
{
    public int FileCounter = 0;

    private bool ready = false;
    private void LateUpdate()
    {
        //captures when pressing F9!
        if (Input.GetKeyDown(KeyCode.F9))
        {
            if(ready) Capture();
        }
    }

    public void setReady()
    {
        ready = true;
    }

    public void resetReady()
    {
        ready = false;
    }


    void Capture()
    {
        Camera MirrorCam = GetComponent<Camera>();

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = MirrorCam.targetTexture;

        MirrorCam.Render();

        Texture2D Image = new Texture2D(MirrorCam.targetTexture.width, MirrorCam.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, MirrorCam.targetTexture.width, MirrorCam.targetTexture.height), 0, 0);
        Image.Apply();
        RenderTexture.active = currentRT;

        var Bytes = Image.EncodeToPNG();
        Destroy(Image);

        File.WriteAllBytes(Application.dataPath + "/Photos/" + FileCounter + ".png", Bytes);
        FileCounter++;
    }
}
