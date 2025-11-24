using Assets.ParadoxShift.Scripts.Input;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.ParadoxShift.Scripts.Rewind
{
    public class RewindManager : MonoBehaviour
    {
        [SerializeField] private InputReader _inputReader;
        private TimeRecorder _recorder;
        private Rigidbody2D _rb;

        public bool isRewinding = false;

        void Start()
        {
            _recorder = GetComponent<TimeRecorder>();
            _rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (_inputReader.IsReward)
            {
                StartRewind();
            }
            else
            {
                StopRewind();
            }
        }

        void FixedUpdate()
        {
            if (isRewinding)
            {
                Rewind();
            }
        }

        void StartRewind()
        {
            isRewinding = true;
            _rb.bodyType = RigidbodyType2D.Kinematic;
        }

        void StopRewind()
        {
            isRewinding = false;
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }

        void Rewind()
        {
            List<TimeRecorder.PlayerState> states = _recorder.GetStates();

            if (states.Count > 0)
            {
                var lastState = states[states.Count - 1];

                transform.SetPositionAndRotation(lastState.position, lastState.rotation);
                _rb.linearVelocity = lastState.velocity;

                states.RemoveAt(states.Count - 1);
            }
        }
    }
}