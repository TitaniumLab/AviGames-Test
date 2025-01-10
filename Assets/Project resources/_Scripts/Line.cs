using System.Collections.Generic;
using UnityEngine;

namespace AviGames
{
    [RequireComponent(typeof(EdgeCollider2D))]
    [RequireComponent(typeof(LineRenderer))]
    public class Line : MonoBehaviour
    {
        private EdgeCollider2D _collider;
        private LineRenderer _renderer;
        [SerializeField] private Transform _startConnection;
        [SerializeField] private Transform _endConnection;
        [SerializeField] private float _indent;
        [SerializeField] private Material _nonOverlapped;
        [SerializeField] private Material _overlapped;


        private void Awake()
        {
            _collider = GetComponent<EdgeCollider2D>();
            _renderer = GetComponent<LineRenderer>();
            SetLine();
        }


        private void OnValidate()
        {
            if (_startConnection && _endConnection)
            {
                _renderer = GetComponent<LineRenderer>();
                _collider = GetComponent<EdgeCollider2D>();
                SetLine();
            }
        }


        private void FixedUpdate()
        {
            SetLine();
            List<Collider2D> results = new List<Collider2D>();
            Physics2D.OverlapCollider(_collider, results);
            if (results.Count > 0)
            {
                Debug.Log("AAAAAAAAAA");
            }
        }


        private void SetLine()
        {
            var mid = (_endConnection.position + _startConnection.position) / 2;
            float halfLenth = (_startConnection.position - mid).magnitude;
            var start = (_startConnection.position - mid).normalized * (halfLenth - _indent) + mid;
            var end = (_endConnection.position - mid).normalized * (halfLenth - _indent) + mid;
            _renderer.SetPosition(0, start);
            _renderer.SetPosition(1, end);
            _collider.points = new Vector2[] { start, end };
        }
    }
}
