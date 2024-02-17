using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static PlayerInputHandler Instance { get; private set; }

    bool mouseDown = false;
    Vector3 firstMousePos;

    void Awake()
    {
        Instance = this;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (mouseDown)
            return;

        if (GameManager.GamePhase == GameManager.GamePhases.Start)
        {
            GameManager.StartToMove();
        }

        mouseDown = true;
        ResetMovementChange();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!mouseDown)
            return;

        mouseDown = false;
        GiveInput(Vector2.zero);
        ResetMovementChange();
    }
    public void GiveInput(Vector2 movementChange)
    {
        PlayerMovementController.Instance.SetMovementChange(movementChange);
    }

    private void Update()
    {
        if (!mouseDown)
            return;

        Vector3 changeOfMousePos = Input.mousePosition - firstMousePos;

        GiveInput(new Vector2(changeOfMousePos.x * 1 / Screen.width, changeOfMousePos.y * 1 / Screen.height));

        ResetMovementChange();

    }
    public void ResetMovementChange()
    {
        firstMousePos = Input.mousePosition;
    }






}
