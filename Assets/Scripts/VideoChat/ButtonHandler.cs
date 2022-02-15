using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{

    /// <summary>
    ///   React to a button click event.  Used in the UI Button action definition.
    /// </summary>
    /// <param name="button"></param>

    private bool isClicked=false;

    public void onButtonClicked(Button button)
    {
        // which GameObject?

        GameObject go = GameObject.Find("GameController");
        if (go != null)
        {
            VideoChatHome gameController = go.GetComponent<VideoChatHome>();
            if (gameController == null)
            {
                Debug.LogError("Missing game controller");
                return;
            }
            if (button.name == "JoinButton")
            {
                if(isClicked==false)
                {
                    gameController.onJoinButtonClicked();
                    isClicked=true;
                }
                else
                {
                    gameController.onLeaveButtonClicked();
                    isClicked=false;
                }
            }
            else if (button.name == "LeaveButton")
            {
                gameController.onLeaveButtonClicked();
            }
        }





    }
}
