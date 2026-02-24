using Assets.Scripts.Core.General;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour, PlayerInput.IPlayerActions
{ 
    public Vector2 PointerPos {  get; private set; } 
    public bool UserTouching { get; private set; }
    public bool FirstSpellCast { get; private set;}
    public bool SecondSpellCast { get; private set; }
    public bool ThirdSpellCast { get; private set; }

    private bool _tapStarted;
    private void Awake()
    {
        InputHandler.EnablePlayerInput();
        InputHandler.SubscribeToPlayerInput(this);
    }
    void Update()
    {
        if (_tapStarted)
        {
            if (!IsPointerOverUI())
            {
                UserTouching = true;
            }
            _tapStarted = false;
        }
    }
    private void OnDisable()
    {
        InputHandler.DisablePlayerInput();
    }
    private bool IsPointerOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventData, results);
        foreach (var result in results)
        {
            if (result.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                return true;
            }
        }

        return false;
    }
    public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
            PointerPos = context.ReadValue<Vector2>();
    }

    public void OnTapCheck(InputAction.CallbackContext context)
    {
        if (context.started)
            _tapStarted = true;

        if (context.canceled)
            UserTouching = false;
    }

    public void OnCastFirstSpell(InputAction.CallbackContext context)
    {
        if (context.started)
            FirstSpellCast = true;

        if (context.canceled)
            FirstSpellCast = false;
    }

    public void OnCastSecondSpell(InputAction.CallbackContext context)
    {
        if (context.started)
            SecondSpellCast = true;

        if (context.canceled)
            SecondSpellCast = false;
    }

    public void OnCastThirdSpell(InputAction.CallbackContext context)
    {
        if (context.started)
            ThirdSpellCast = true;

        if (context.canceled)
            ThirdSpellCast = false;
    }

}
