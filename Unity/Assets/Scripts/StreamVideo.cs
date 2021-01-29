using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamVideo : MonoBehaviour
{
    public RawImage OriginalImage;
    public RawImage ModifiedImage;
    public VideoPlayer videoPlayer;

    // Use this for initialization
    void Start()
    {
        videoPlayer.sendFrameReadyEvents = true;
        videoPlayer.frameReady += OnNewFrame;
    }

    void OnNewFrame(VideoPlayer source, long frameIdx)
    {
        //Visualizza il fotogramma del video sulla texture di OriginalImage
        OriginalImage.texture = videoPlayer.texture;

        //Il blocco di istruzioni successivo crea una Texture2D (necessaria per creare una Mat di OpenCV)
        //a partire dalla RenderTexture restituita dal videoPlayer
        RenderTexture videoFrame = videoPlayer.texture as RenderTexture;
        RenderTexture.active = videoFrame;
        Texture2D texture = new Texture2D(videoFrame.width, videoFrame.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new UnityEngine.Rect(0, 0, videoFrame.width, videoFrame.height), 0, 0, false);
        RenderTexture.active = null;
        //... e distrutta la Texture2D temporanea usata per crearla. In questo modo si libera memoria e si evita un memory leak.
        Destroy(texture);
    }

}