using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager sharedInstance;

    public static InputManager SharedInstance => sharedInstance;

    private RunnerInputAction actionScheme;

    [SerializeField] private float sqrtSwipeDeadzone = 50f;

    #region public properties
    public bool Tap => tap;
    public Vector2 TouchPosition => touchPosition;
    public bool LeftSwipe => leftSwipe;
    public bool RightSwipe => rightSwipe;
    public bool UpSwipe => upSwipe;
    public bool DownSwipe => downSwipe;
    #endregion

    #region privates
    private bool tap;
    private Vector2 touchPosition;
    private Vector2 startDrag;
    private bool leftSwipe;
    private bool rightSwipe;
    private bool upSwipe;
    private bool downSwipe;

    #endregion

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        SetupControl();
    }

    private void LateUpdate()
    {
        ResetInputs();
    }

    private void ResetInputs()
    {
        tap = false;
        leftSwipe = false;
        rightSwipe = false;
        upSwipe = false;
        downSwipe = false;
    }

    private void SetupControl()
    {
        actionScheme = new RunnerInputAction();

        actionScheme.Gameplay.Tap.performed += ctx => OnTap(ctx);
        actionScheme.Gameplay.TouchPosition.performed += ctx => OnTouchPosition(ctx);
        actionScheme.Gameplay.StartDrag.performed += ctx => OnStartDrag(ctx);
        actionScheme.Gameplay.EndDrag.performed += ctx => OnEndDrag(ctx);
    }

    private void OnTap(InputAction.CallbackContext context)
    {
        tap = true;
    }

    private void OnTouchPosition(InputAction.CallbackContext context)
    {
        touchPosition = context.ReadValue<Vector2>();
    }

    private void OnStartDrag(InputAction.CallbackContext context)
    {
        startDrag = touchPosition;
    }

    private void OnEndDrag(InputAction.CallbackContext context)
    {
        Vector2 delta = touchPosition - startDrag;
        float sqrtDistance = delta.sqrMagnitude;

        if (sqrtDistance > sqrtSwipeDeadzone)
        {
            
        }
    }

    private void OnEnable()
    {
        actionScheme.Enable();
    }

    private void OnDisable()
    {
        actionScheme.Disable();
    }


}
