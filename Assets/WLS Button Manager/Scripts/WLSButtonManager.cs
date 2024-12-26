using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class WLSButtonManager : MonoBehaviour
{
    private List<WLSButton> wlsButtons = new List<WLSButton>();

    void Start()
    {
        GetWLSButtons();
    }

    void Update()
    {
        TabInput();
    }

    private void GetWLSButtons()
    {
        // Get the list of buttons in the scene
        foreach (var go in GameObject.FindGameObjectsWithTag("WLSButton"))
        {
            wlsButtons.Add(go.GetComponent<WLSButton>());
        }

        // Sort the list of buttons by assigned tab order
        wlsButtons.Sort((x, y) => x.transform.parent.GetSiblingIndex().CompareTo(y.transform.parent.GetSiblingIndex()));

        bool buttonHasFocus = false;

        // Check that TabOrder is properly assigned to each
        for (var i = 0; i < wlsButtons.Count; i++)
        {
            if (wlsButtons[i].HasFocus)
            {
                buttonHasFocus = true;
            }
        }
        if (!buttonHasFocus)
        {
            Debug.Log("ERROR - WLS Buttons: No button was given focus.");
        }
    }

    private void TabInput()
    {
        // Tab through the buttons
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                SelectButtonTabOrder(-1);
            }
            else
            {
                SelectButtonTabOrder(1);
            }
        }
    }

    void SelectButtonTabOrder(int direction)
    {
        WLSButton nextButton;

        // Get the button that currently has focus
        WLSButton currentButton = wlsButtons.FirstOrDefault(x => x.HasFocus == true);
        int currentIndex = wlsButtons.IndexOf(currentButton);
        int nextTabOrder = currentIndex + direction;

        // Select the next button
        if (nextTabOrder == wlsButtons.Count)
        {
            // Wrap back to the first button
            nextButton = wlsButtons[0];
        }
        else if (nextTabOrder < 0)
        {
            // Wrap forward to the last button
            nextButton = wlsButtons[wlsButtons.Count - 1];
        }
        else
        {
            // Select the next button
            nextButton = wlsButtons[nextTabOrder];
        }

        SelectButton(nextButton);
    }

    public void SelectButton(WLSButton nextButton)
    {
        // Loop through all buttons and set focus
        foreach (var btn in wlsButtons)
        {
            if (btn == nextButton)
            {
                btn.HasFocus = true;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(btn.gameObject);
            }
            else
            {
                btn.HasFocus = false;
            }
        }
    }

    // This is function can be called externally to re-select the button that currently has focus
    // Useful after showing a popup menu in game
    public void SelectCurrentButton()
    {
        // Loop through all buttons and set focus
        foreach (var btn in wlsButtons)
        {
            if (btn.HasFocus)
            {
                // Workaround for Unity's current UI behavior. Deselect the current game object first, then reselect it.
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(btn.gameObject);
                break;
            }
        }
    }
}