using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AviGames
{
    [RequireComponent(typeof(EdgeCollider2D))]
    [RequireComponent(typeof(LineRenderer))]
    public class Line : MonoBehaviour
    {
        private EdgeCollider2D _collider;
        private LineRenderer _renderer;
        [SerializeField] private Knot _startConnection;
        [SerializeField] private Knot _endConnection;
        private Transform _startTransform, _endTransform;
        [SerializeField] private float _indent;
        [SerializeField] private Material _nonOverlappedMat;
        [SerializeField] private Material _overlappedMat;
        private ContactFilter2D _filter;
        private static List<Line> _lines = new List<Line>();
        private bool _isOverlaped;



        private void Awake()
        {
            _collider = GetComponent<EdgeCollider2D>();
            _renderer = GetComponent<LineRenderer>();
            _startTransform = _startConnection.transform;
            _endTransform = _endConnection.transform;

            _startConnection.OnDragging += SetLine;
            _endConnection.OnDragging += SetLine;
            Knot.OnRelease += SetLine;
            Knot.OnRelease += CheckOverlap;

            _filter.SetLayerMask(1 << gameObject.layer);
            _filter.useLayerMask = true;
            _lines.Add(this);

            SetLine();
            CheckOverlap();
        }


        private void OnValidate()
        {
            if (_startConnection && _endConnection)
            {
                _startTransform = _startConnection.transform;
                _endTransform = _endConnection.transform;
                _renderer = GetComponent<LineRenderer>();
                _collider = GetComponent<EdgeCollider2D>();
                SetLine();
            }
        }


        private void OnDestroy()
        {
            _startConnection.OnDragging -= SetLine;
            _endConnection.OnDragging -= SetLine;
            Knot.OnRelease -= CheckOverlap;
            Knot.OnRelease -= SetLine;
            if (_lines.Count > 0)
            {
                _lines.Clear();
            }
        }


        private void SetLine()
        {
            var dir = _endTransform.position - _startTransform.position;
            var start = _startTransform.position + dir.normalized * _indent;
            var end = _endTransform.position - dir.normalized * _indent;
            _renderer.SetPosition(0, start);
            _renderer.SetPosition(1, end);
            _collider.points = new Vector2[] { start, end };
        }


        private void CheckOverlap()
        {

            List<Collider2D> results = new List<Collider2D>();
            Physics2D.OverlapCollider(_collider, _filter, results);
            if (results.Count > 0)
            {
                _renderer.material = _overlappedMat;
                _isOverlaped = true;
            }
            else
            {
                _renderer.material = _nonOverlappedMat;
                _isOverlaped = false;
            }
        }


        public static bool CheckPuzzleSolved()
        {
            if (_lines.All(line => !line._isOverlaped))
            {
                return true;
            }
            return false;
        }
    }
}
