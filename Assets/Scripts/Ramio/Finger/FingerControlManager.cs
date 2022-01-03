using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerControlManager : MonoBehaviour
{
    [SerializeField] GameObject fingerCollider = null;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0f;

                fingerCollider.transform.position = touchPosition;

                fingerCollider.gameObject.SetActive(true);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                fingerCollider.gameObject.SetActive(false);
            }
        }
    }
}
