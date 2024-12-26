using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WLSButton : MonoBehaviour, IPointerEnterHandler
{
    [Tooltip("Assign this to the default button in the scene")]
    public bool HasFocus;

    private WLSButtonManager wlsButtonManager;

    private Button thisButton;

    void Start()
    {
        // Verify that there is a button manager in the scene
        try
        {
            wlsButtonManager = GameObject.FindGameObjectWithTag("WLSButtonManager").GetComponent<WLSButtonManager>();
        }
        catch
        {
            Debug.Log("ERROR - WLS Buttons: No WLS Button Manager in the scene.");
        }

        // Get a reference to the Unity button UI script on this game object
        thisButton = gameObject.GetComponent<Button>();

        // If this button has focus, call Unity button UI select
        if (HasFocus)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(thisButton.gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        wlsButtonManager.SelectButton(gameObject.GetComponent<WLSButton>());
    }
}