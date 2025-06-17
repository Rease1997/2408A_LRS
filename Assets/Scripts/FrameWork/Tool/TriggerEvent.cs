using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TriggerEvent : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public Action TiggerEnter2D;
    public Action<Collider2D> TiggerStay2D;
    public Action TiggerExit2D;
    public Action DragAction;
    public Action BeginDragAction;
    public Action EndDragAction;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TiggerEnter2D != null)
            TiggerEnter2D.Invoke();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (TiggerStay2D != null)
            TiggerStay2D.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (TiggerExit2D != null)
            TiggerExit2D.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (DragAction != null)
            DragAction.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (BeginDragAction != null)
            BeginDragAction.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EndDragAction != null)
            EndDragAction.Invoke();
    }
}
