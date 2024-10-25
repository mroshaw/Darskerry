using DaftAppleGames.Darskerry.Core.PlayerController;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerCameraInputTest : MonoBehaviour //, PlayerControls.ICameraControlsActions
{

    #region Properties
    private PlayerCamera _playerCamera;
    private Vector2 _lookInputValue = Vector2.zero;

    public PlayerControls PlayerControls { get; private set; }
    #endregion

    #region Startup
    private void Awake()
    {
        if (PlayerControls == null)
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();
        }
    }

    private void OnEnable()
    {
        if (PlayerControls == null)
        {
            Debug.LogError("Player controls not initialized - cannot enable");
            return;
        }
        // PlayerControls.CameraControls.Enable();
        // PlayerControls.CameraControls.SetCallbacks(this);

    }

    private void OnDisable()
    {
        PlayerControls.Disable();
    }

    #endregion

    #region Updates
    private void Update()
    {
        // FIX - CALL LOOK EVERY FRAME
        _playerCamera.Look(_lookInputValue);
    }
    #endregion

    #region Input Callbacks
    public void OnLook(InputAction.CallbackContext context)
    {
        // FIX - SET LOOK VALUE WHEN A CHANGE IS RECEIVED
        _lookInputValue = context.ReadValue<Vector2>();

    }
    #endregion
}