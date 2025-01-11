using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AviGames
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Button _skipButton;
        [SerializeField] private GameObject _levelCompleteScreen;
        [SerializeField] private ParticleSystem[] _particles;
        private bool _isSkipped;

        private void Awake()
        {
            _isSkipped = false;
            Knot.OnReleaseEnd += CheckCompletion;
        }

        private void OnDestroy()
        {
            Knot.OnReleaseEnd -= CheckCompletion;
        }

        public async void OnSkipButtonPress()
        {
            _isSkipped = true;
            _skipButton.interactable = false;
            await Knot.OnSkip();
        }

        private void CheckCompletion()
        {
            if (Line.CheckPuzzleSolved())
            {
                if (_isSkipped)
                {
                    _levelCompleteScreen.SetActive(true);
                }
                else
                {
                    _levelCompleteScreen.SetActive(true);
                    foreach (var item in _particles)
                    {
                        item.Play();
                    }
                }
            }
        }


        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
