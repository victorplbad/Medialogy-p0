using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "VideoBehavior", menuName = "UI/VideoBehavior")]
public class VideoBehavior : ScriptableObject
{
    public VideoClip VideoClip;
    public RenderTexture RenderTexture;

    private VideoPlayer _videoPlayer;

    public void OnClick(VideoController controller)
    {
        var videoPlayer = controller.GetComponent<VideoPlayer>();

        if (_videoPlayer == null)
        {
            Debug.LogError("VideoPlayer not set. Call SetVideo first.");
            return;
        }

        if (_videoPlayer.isPlaying)
        {
            _videoPlayer.Pause();

            Debug.Log("Video Paused");
            return;
        }

        videoPlayer.Play();

        Debug.Log("Video Playing");
    }

    public void SetVideo(VideoController controller)
    {
        _videoPlayer = controller.AddComponent<VideoPlayer>();
        _videoPlayer.playOnAwake = false;

        _videoPlayer.clip = VideoClip;
        _videoPlayer.targetTexture = RenderTexture;

        ClearOutRenderTexture(RenderTexture);
    }

    private void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.black);
        RenderTexture.active = null;
    }
}
