using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class edgeDetector2 : MonoBehaviour
{
    

    private Camera thisCamera;
    private int currentWidth;
    private int currentHeight;

    private string _globalTextureName = "_GlobalEdgeText";

    // Use this for initialization
    void Start()
    {
        thisCamera = GetComponent<Camera>();
        thisCamera.depthTextureMode = DepthTextureMode.DepthNormals;

        if (thisCamera.targetTexture != null)
        {
            RenderTexture temp = thisCamera.targetTexture;
            thisCamera.targetTexture = null;
            DestroyImmediate(temp);
        }

        thisCamera.targetTexture = new RenderTexture(thisCamera.pixelWidth, thisCamera.pixelHeight, 16);
        thisCamera.targetTexture.filterMode = FilterMode.Bilinear;

        //Shader.SetGlobalTexture(_globalTextureName, thisCamera.targetTexture);



    }



    // Update is called once per frame
    void Update()
    {
        if (currentHeight != Screen.currentResolution.height || currentWidth != Screen.currentResolution.width)
        {
            currentHeight = Screen.height;
            currentWidth = Screen.width;
            //SetupRT();
        }
    }
}