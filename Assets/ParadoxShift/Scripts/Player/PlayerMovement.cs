using Assets.ParadoxShift.Scripts.Input;
using System;
using UnityEngine;

namespace Assets.ParadoxShift.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Move")]
        [SerializeField] private float _moveSpeed = 6f;
        [Header("Jump")]
        [SerializeField] private float _jumpForce;
        [SerializeField] private Transform _groundCheckerTransform;
        [SerializeField] private Vector2 _groundCheckerSize;
        [SerializeField] private LayerMask _groundLayerMask;
        private bool _isGrounded;
        private bool _isJumping;
        [Header("Systems")]
        [SerializeField] private InputReader _inputReader;
        [Header("Gravity")]
        [SerializeField] private float _fallMultiplier = 2.5f;
        [SerializeField] private float _lowJumpMultiplier = 2f;
        [Header("Jump Feel")]
        [SerializeField] private float _coyoteTime = 0.2f;
        [SerializeField] private float _jumpBufferTime = 0.2f;
        private float _coyoteTimeCounter;
        private float _jumpBufferCounter;

        private Rigidbody2D _rb;
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _inputReader.EnablePlayerAction();
            _inputReader.Jump += isJumpingKeyPressed =>
            {
                if (isJumpingKeyPressed)
                {
                    Jump();
                }
                else
                {
                    Land();
                }
            };
            
        }
        private void Update()
        {
            if (_isGrounded)
            {
                _coyoteTimeCounter = _coyoteTime;
            }
            else
            {
                _coyoteTimeCounter -= Time.deltaTime;
            }
            _jumpBufferCounter -= Time.deltaTime;

        }
        private void FixedUpdate()
        {
            _rb.linearVelocity = new Vector2(_inputReader.Direction.x * _moveSpeed, _rb.linearVelocity.y);
            CheckGround();
            HandleGravity();

            if (_coyoteTimeCounter > 0f && _jumpBufferCounter > 0f)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
                _jumpBufferCounter = 0f;
                _coyoteTimeCounter = 0f;
            }
        }

        private void HandleGravity()
        {
            // Si el jugador esta cayendo
            if (_rb.linearVelocity.y < 0)
            {
                _rb.gravityScale = _fallMultiplier;
            }
            // Si esta subiendo pero solto el boton de salto
            else if (_rb.linearVelocity.y > 0 && !_inputReader.IsJumpHeld)
            {
                _rb.gravityScale = _lowJumpMultiplier;
            }
            else
            {
                // Gravedad normal
                _rb.gravityScale = 1f;
            }
        }
        private void Land()
        {
             _jumpBufferCounter -= Time.deltaTime;
        }

        private void Jump()
        {
            _jumpBufferCounter = _jumpBufferTime;
            if (_isGrounded)
            {
                _isJumping = true;
            }
            

        }
        private void TryJump()
        {
            if (_isJumping)
            {
                _isJumping = false;
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
            }
        }
        private void CheckGround()
        {
            _isGrounded = Physics2D.OverlapBox(_groundCheckerTransform.position, _groundCheckerSize, 0, _groundLayerMask);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_groundCheckerTransform.position, _groundCheckerSize);
        }
    }
}