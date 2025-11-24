using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

namespace Assets.ParadoxShift.Scripts.Input
{
    public interface IInputReader
    {
        Vector2 Direction { get; }
        void EnablePlayerAction();
    }
    [CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
    public class InputReader : ScriptableObject, IPlayerActions, IInputReader
    {
        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<bool> Jump = delegate { };
        public event UnityAction<bool> Reward = delegate { };
        public InputSystem_Actions inputActions;
        public Vector2 Direction => inputActions.Player.Move.ReadValue<Vector2>();
        public bool IsJumpHeld => inputActions.Player.Jump.IsPressed();
        public bool IsReward => inputActions.Player.Reward.IsPressed();

        public void EnablePlayerAction()
        {
            if (inputActions == null)
            {
                inputActions = new InputSystem_Actions();
                inputActions.Player.SetCallbacks(this);
            }
            inputActions.Enable();
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            Move.Invoke(context.ReadValue<Vector2>());
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Jump.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Jump.Invoke(false);
                    break;
            }
        }
        public void OnAttack(InputAction.CallbackContext context)
        {
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
        }



        public void OnLook(InputAction.CallbackContext context)
        {
        }



        public void OnNext(InputAction.CallbackContext context)
        {
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
        }

        public void OnReward(InputAction.CallbackContext context)
        {
        }
    }
}