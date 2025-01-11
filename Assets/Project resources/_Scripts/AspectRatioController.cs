using UnityEngine;

namespace AviGames
{
    public class AspectRatioController : MonoBehaviour
    {
        [SerializeField] private Vector2 _targetAspectRatio = new Vector2(16, 9);
        private Vector2 _defaultRectSize = new Vector2(1, 1);
        private Vector2 _rectCenter = new Vector2(0.5f, 0.5f);
        private Vector2 _previousScreen;

        private void Awake()
        {
            SetCamera();
        }

        private void Update()
        {
            SetCamera();
        }


        public void SetCamera()
        {
            Vector2 currentScreen = new((float)Screen.width, (float)Screen.height);
            if (_previousScreen != currentScreen)
            {
                //// If screen higher then target camera view
                if (currentScreen.x / currentScreen.y < _targetAspectRatio.x / _targetAspectRatio.y)
                {
                    float pixelScale = Screen.width / _targetAspectRatio.x;
                    float targetHeight = pixelScale * _targetAspectRatio.y;
                    float relativeHeight = targetHeight / Screen.height;
                    Vector2 rectSize = new Vector2(_defaultRectSize.x, relativeHeight);
                    Camera.main.rect = new Rect(default, rectSize) { center = _rectCenter };
                    Debug.Log($"New Camera Rect: {rectSize}");
                }
                // If screen wider then target camera view
                else
                {
                    float pixelScale = Screen.height / _targetAspectRatio.y;
                    float targetWidth = pixelScale * _targetAspectRatio.x;
                    float relativeWidth = targetWidth / Screen.width;
                    Vector2 rectSize = new Vector2(relativeWidth, _defaultRectSize.y);
                    Camera.main.rect = new Rect(default, rectSize) { center = _rectCenter };
                    Debug.Log($"New Camera Rect: {rectSize}");
                }
                _previousScreen = currentScreen;
            }
        }
    }
}
