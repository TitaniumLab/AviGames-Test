using UnityEngine;

namespace AviGames
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _sfxSourse; // Просрал пол часа исправляя задержку в аудио при клике, чтобы узнать, что у блютус наушников есть задержка
        [SerializeField] private AudioClip _onClickAudio;
        [SerializeField] private AudioClip _ropeSound;
        [Range(0, 1)]
        [SerializeField] private float _volumeChangeMulty = 0.2f;
        [Range(0, 1)]
        [SerializeField] private float _pitchChangeMulty = 0.2f;


        private void Awake()
        {
            Knot.OnClick += PlayClick;
            Knot.OnAnyDragging += PlayRope;
            Knot.OnAnyEndDrag += _sfxSourse.Stop;
            Knot.OnRelease += _sfxSourse.Stop;
        }


        private void OnDestroy()
        {
            Knot.OnClick -= PlayClick;
            Knot.OnAnyDragging -= PlayRope;
            Knot.OnAnyEndDrag -= _sfxSourse.Stop;
            Knot.OnRelease -= _sfxSourse.Stop;

        }

        private void PlayClick()
        {
            _sfxSourse.volume = 1;
            _sfxSourse.clip = _onClickAudio;
            _sfxSourse.Play();
        }


        private void PlayRope()
        {
            if (!_sfxSourse.isPlaying)
            {
                _sfxSourse.volume = Random.Range(1 - _volumeChangeMulty, 1.0f);
                _sfxSourse.pitch = Random.Range(1 - _pitchChangeMulty, 1 + _pitchChangeMulty);
                _sfxSourse.clip = _ropeSound;
                _sfxSourse.Play();
            }
        }
    }
}
