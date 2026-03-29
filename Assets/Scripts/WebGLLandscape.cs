using UnityEngine;

namespace Daadab
{
    public class WebGLLandscape : MonoBehaviour
    {
        void Start()
        {
            // This works better for Unity 6 WebGL Fullscreen transitions
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }
}
