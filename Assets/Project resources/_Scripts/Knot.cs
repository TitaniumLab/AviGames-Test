using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace AviGames
{
    [RequireComponent(typeof(Collider2D))]
    public class Knot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler, IDragHandler
    {
        // Под конец тестового задания, понял, что следовало сделать расстановку узелков через scriptableObject, но уже поздно
        [SerializeField] private SpriteRenderer _outLine;
        [SerializeField] private SpriteRenderer _shadow;
        private static List<Knot> _knots = new List<Knot>();
        private Collider2D _collider;
        private static bool _isDragging;
        private Camera _camera;
        private PlayArea _playArea;
        public static event Action OnClick;
        public static event Action OnRelease;
        public static event Action OnReleaseEnd; // The event is necessary for the correct order of calling checks. It's better to use event bus
        public static event Action OnAnyEndDrag;
        public static event Action OnAnyDragging;
        public event Action OnDragging;


        [Inject]
        private void Construct(PlayArea playArea)
        {
            _playArea = playArea;
        }


        private void Awake()
        {
            _isDragging = false;
            _knots.Add(this);
            _outLine.enabled = false;
            _shadow.enabled = false;
            _collider = GetComponent<Collider2D>();
            _camera = Camera.main;
        }


        private void OnDestroy()
        {
            if (_knots.Count > 0)
            {
                _knots.Clear();
            }
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isDragging)
            {
                _outLine.enabled = true;
            }
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isDragging)
            {
                _outLine.enabled = false;
            }
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            OnClick?.Invoke();
            _isDragging = true;
            _outLine.enabled = false;
            _shadow.enabled = true;
            transform.parent.SetAsFirstSibling();
        }


        public void OnPointerUp(PointerEventData eventData)
        {
            _isDragging = false;
            _outLine.enabled = true;
            _shadow.enabled = false;
            ReturnToPlayArea();
            OnRelease?.Invoke();
            OnReleaseEnd?.Invoke();
        }


        public void OnDrag(PointerEventData eventData)
        {
            if (_isDragging)
            {
                var newPos = _camera.ScreenToWorldPoint(eventData.position, Camera.MonoOrStereoscopicEye.Mono);
                newPos[2] = 0;
                transform.position = newPos;
                OnAnyDragging?.Invoke();
                OnDragging?.Invoke();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnAnyEndDrag?.Invoke();
            // Prevents a bug when releasing on quick mouse movement
            if (eventData.pointerCurrentRaycast.gameObject != this)
            {
                _isDragging = false;
                _outLine.enabled = false;
                _shadow.enabled = false;
                ReturnToPlayArea();
                OnRelease?.Invoke();
                OnReleaseEnd?.Invoke();
            }
        }


        public static async Task OnSkip()
        {
            List<Task> tasks = new List<Task>();
            foreach (Knot item in _knots)
            {
                _isDragging = true;
                item._collider.enabled = false;
                var tween = item.transform.DOMove(item.transform.parent.position, 1);
                tween.onUpdate += () =>
                {
                    OnAnyDragging?.Invoke();
                    item.OnDragging?.Invoke();
                };
                tween.onComplete += () => {; };
                tasks.Add(tween.AsyncWaitForCompletion());
            }
            await Task.WhenAll(tasks);
            OnRelease?.Invoke();
            OnReleaseEnd?.Invoke();
        }


        private void ReturnToPlayArea()
        {
            if (transform.position.y > _playArea.PlayAreaRect.height / 2)
            {
                var newPos = transform.position;
                newPos[1] = _playArea.PlayAreaRect.height / 2;
                transform.position = newPos;
            }
            else if (transform.position.y < -_playArea.PlayAreaRect.height / 2)
            {
                var newPos = transform.position;
                newPos[1] = -_playArea.PlayAreaRect.height / 2;
                transform.position = newPos;
            }

            if (transform.position.x > _playArea.PlayAreaRect.width / 2)
            {
                var newPos = transform.position;
                newPos[0] = _playArea.PlayAreaRect.width / 2;
                transform.position = newPos;
            }
            else if (transform.position.x < -_playArea.PlayAreaRect.width / 2)
            {
                var newPos = transform.position;
                newPos[0] = -_playArea.PlayAreaRect.width / 2;
                transform.position = newPos;
            }
        }
    }
}
