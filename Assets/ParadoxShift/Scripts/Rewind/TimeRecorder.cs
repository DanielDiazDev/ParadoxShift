using System.Collections.Generic;
using UnityEngine;

namespace Assets.ParadoxShift.Scripts.Rewind
{
    public class TimeRecorder : MonoBehaviour
    {
        [System.Serializable]
        public struct PlayerState
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector2 velocity;
        }
        public float recordTime = 5f;
        private List<PlayerState> _states = new List<PlayerState>();
        private Rigidbody2D _rb;
        private RewindManager _rewindManager;
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rewindManager = GetComponent<RewindManager>();
        }

        void FixedUpdate()
        {
            if (!_rewindManager.isRewinding)
            {
                RecordState();
            }
        }
        void RecordState()
        {
            Debug.Log(_states.Count);
            if (_states.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
            {
                _states.RemoveAt(0);
            }

            PlayerState state = new PlayerState
            {
                position = transform.position,
                rotation = transform.rotation,
                velocity = _rb.linearVelocity
            };

            _states.Add(state);
        }

        public List<PlayerState> GetStates()
        {
            return _states;
        }
    }
}