using UnityEngine;

namespace AviGames
{
    public class PlayArea : MonoBehaviour
    {
        public Rect PlayAreaRect { get; private set; }

        private void Awake()
        {
            PlayAreaRect = new Rect(transform.position, transform.localScale);
        }
    }
}
