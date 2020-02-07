using UnityEngine;

public class Marker : MonoBehaviour
{
    public bool selected;

    public string NameCounty;
    UIController uicontroller;

    private void Start()
    {
        uicontroller = GameObject.Find("UIController").GetComponent<UIController>();
    }


    public void ShowModel()
    {
        if (transform.childCount > 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        uicontroller.ActivateInfoPanel(NameCounty);
    }

    public void SelectCountry() {
        if (!selected)
        {
            if (transform.childCount > 0)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(true);
                selected = true;
            }
            uicontroller.ActivateSelectedCountrysPanel(NameCounty, this.gameObject);
        }
    }
}
