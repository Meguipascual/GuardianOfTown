using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MultiTouchDisplay : MonoBehaviour
{
    public Text multiTouchInfoDisplay;
    private int maxTapCount = 0;
    private string multiTouchInfo;
    private Touch theTouch;

    // Update is called once per frame
    void Update()
    {
        multiTouchInfo = string.Format($"Max tap count: {maxTapCount}\n");

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                theTouch = Input.GetTouch(i);

                multiTouchInfo +=
                    string.Format($"Touch {i.ToString()} - Position {theTouch.position} - Tap Count: {theTouch.tapCount} - Finger ID: {theTouch.fingerId}\nRadius: {theTouch.radius}");

            if (theTouch.tapCount > maxTapCount)
            {
                maxTapCount = theTouch.tapCount;
            }
        }
    }

    multiTouchInfoDisplay.text = multiTouchInfo;
    }
}
