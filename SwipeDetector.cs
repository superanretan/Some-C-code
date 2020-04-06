using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerSwipeDetector : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    public bool touched, timeCalcOn, swipeLock, checkPosition;

    public float maxMoveDistance, maxMoveTime, actualTime;

    public GameObject startTouchPos;

    public float minSwipeDistY;

    public float minSwipeDistX;

    public float positionChange;

    private void Start()
    {
        swipeLock = false;

        touched = false;
        timeCalcOn = false;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        timeCalcOn = false;
        touched = false;
        swipeLock = false;
        checkPosition = false;
        positionChange = 0;
        actualTime = 0;
        if (!Player.instance.rotate) Player.instance.rotateZeroPlayer();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        touched = true;
        startTouchPos.transform.position = Input.mousePosition;
    }

    void Update()
    {

        timeCalc();
        positionCheck();

        if (touched)
        {
            checkPosition = true;
            timeCalcOn = true;

            if (actualTime > maxMoveTime && positionChange < maxMoveDistance)
            {
                swipeLock = true;
                Player.instance.Movement();
                checkPosition = false;
                timeCalcOn = false;
            }
            else
            {
                if (Input.touchCount > 0 && !swipeLock)
                {
                    var cm = Player.instance;

                    Touch touch = Input.touches[0];

                    float swipeDistHorizontal = (new Vector3(touch.position.x, 0, 0) - new Vector3(startTouchPos.transform.position.x, 0, 0)).magnitude;

                    if (swipeDistHorizontal > minSwipeDistX)

                    {

                        float swipeValue = Mathf.Sign(touch.position.x - startTouchPos.transform.position.x);

                        if (swipeValue > 0)
                        {
                            cm.PushRight();
                            swipeLock = true;
                        }//right swipe

                        else if (swipeValue < 0)
                        {
                            cm.PushLeft();
                            swipeLock = true;
                           
                        }//left swipe
                    }
                }
            }
        }
    }

  

    void positionCheck()
    {
        if (checkPosition)
        {
            positionChange = Mathf.Abs((new Vector2(startTouchPos.transform.position.x, 0) - new Vector2(Input.mousePosition.x, 0)).magnitude);
        }
    }

    void timeCalc()
    {
        if (timeCalcOn)
        {
            actualTime += Time.deltaTime;
        }
    }
}

