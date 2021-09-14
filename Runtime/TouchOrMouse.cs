using System;
using UnityEngine;

public class TouchOrMouse
{
  public static int touchCount
  {
    get
    {
      if (Input.touchCount > 0) { return Input.touchCount; }
      if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
      {
        return 1;
      }
      return 0;
    }
  }

  private static Touch _mouseTouch;
  public static Touch mouseTouch
  {
    get { return _mouseTouch; }
  }

  public static Touch GetTouch(int i)
  {
    if (Input.touchCount > 0) { return Input.GetTouch(i); }
    _mouseTouch.phase = TouchPhase.Canceled;
    if (Input.GetMouseButtonDown(0)) { _mouseTouch.phase = TouchPhase.Began; }
    else if (Input.GetMouseButtonUp(0)) { _mouseTouch.phase = TouchPhase.Ended; }
    else if (Input.GetMouseButton(0)) {
      if (_mouseTouch.position == Input.mousePosition.XY())
      {
        _mouseTouch.phase = TouchPhase.Stationary;
      } else
      {
        _mouseTouch.phase = TouchPhase.Moved;
      }
    } else
    {
      _mouseTouch.phase = TouchPhase.Canceled;
    }
    _mouseTouch.position = Input.mousePosition; 
    return mouseTouch;
  } 
}
