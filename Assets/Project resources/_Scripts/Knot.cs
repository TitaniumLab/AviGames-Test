using UnityEngine;

namespace AviGames
{
    public class Knot : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _outLine;
        [SerializeField] private SpriteRenderer _shadow;

        private void Awake()
        {
            _outLine.enabled = false;
            _shadow.enabled = false;
        }


        private void OnMouseEnter()
        {
            _outLine.enabled = true;
        }


        private void OnMouseExit()
        {
            
        }


        


        private void ActivateOutline()
        {


        }


        private void ActivateShadow()
        {

        }
    }
}
