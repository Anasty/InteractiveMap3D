using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject prefabCountryPanel;
    public Button ClearButton, BackButton, ListButton, prefabSortButton;

    CameraController cam;

    GameObject infoPanel, selectedCountrysPanel, windowSelectedCountrys;
    Text spaceText, gdpText, populationText, numberSelectedCountysText;

    List<County> countrys;
    List<GameObject> CountryPanels = new List<GameObject>();
    List<GameObject> ActiveMarkers ;
    List<Button> sortButtons = new List<Button>();

    int numberSelectedCountrys = 0;
    int CountCountryPanels = 1;
    int CountSortButton = 0;

    class County
    {
        public bool selected;
        public string name;
        public int space;
        public float gdp;
        public int population;

        public County(string name, int space, float gdp, int population)
        {
            this.name = name;
            this.space = space;
            this.gdp = gdp;
            this.population = population;
        }
    }

    private void Start()
    {
        cam = Camera.main.GetComponent<CameraController>();

        infoPanel = GameObject.Find("PanelInfo");
        selectedCountrysPanel = GameObject.Find("SelectedCountrysPanel");
        windowSelectedCountrys = GameObject.Find("WindowSelectedCountrys");

        spaceText = GameObject.Find("SpaceText").GetComponent<Text>();
        gdpText = GameObject.Find("GDPText").GetComponent<Text>();
        populationText = GameObject.Find("PopulationText").GetComponent<Text>();
        numberSelectedCountysText = GameObject.Find("SelectedText").GetComponent<Text>();

        infoPanel.SetActive(false);
        selectedCountrysPanel.SetActive(false);
        windowSelectedCountrys.SetActive(false);

        ClearButton.gameObject.SetActive(false);
      
        CreateSortButton("По площади");
        CreateSortButton("По населению");
        CreateSortButton("По ВВП");

        ListButton.onClick.AddListener(ButtonListOnClick);
        BackButton.onClick.AddListener(ButtonBackOnClick);
        ClearButton.onClick.AddListener(ButtonClearOnClick);


        countrys = new List<County>() {
            new County("Франция",643801,2.583f,66990000),
            new County("Россия",17100000,1.578f,144500000),
            new County("Швеция",450295,0.538f,10120000),
            new County("Китай",9597000,12.24f,1386000000)
        };

        ActiveMarkers = new List<GameObject>();
    }

    void CreateSortButton(string text)
    {
        int indent = 45;
        Button newButton = Instantiate(prefabSortButton, windowSelectedCountrys.transform);
        newButton.GetComponent<RectTransform>().offsetMax -= new Vector2(0, indent * CountSortButton);
        newButton.GetComponent<RectTransform>().offsetMin -= new Vector2(0, indent * CountSortButton);
     //   newButton.transform.position -= new Vector3(0,indent * CountSortButton, 0);
        newButton.transform.GetChild(0).GetComponent<Text>().text = text;
        newButton.onClick.AddListener(() => SortButtonOnClick(text, newButton));
        sortButtons.Add(newButton);
        CountSortButton++;
    }    

    public void ActivateInfoPanel(string NameCountry)
    {
        infoPanel.SetActive(true);
        for (int i = 0; i < countrys.Count; i++)
        {
            if (countrys[i].name == NameCountry)
            {              
                spaceText.text = "Площадь " + countrys[i].space + " КМ2";
                gdpText.text = "ВВП " + countrys[i].gdp + " трлн.долл";
                populationText.text = "Население " + countrys[i].population + " чел";
            }
        }
    }
    public void ActivateSelectedCountrysPanel(string NameCountry, GameObject gpsMarker)
    {
        ClearButton.gameObject.SetActive(true);
        ActiveMarkers.Add(gpsMarker);
        numberSelectedCountrys++;
        infoPanel.SetActive(false);
        selectedCountrysPanel.SetActive(true);
        numberSelectedCountysText.text = "Выбранно " + numberSelectedCountrys + " стран";

        for (int i = 0; i < countrys.Count; i++)
        {
            if (countrys[i].name == NameCountry)
            {
                countrys[i].selected = true;
            }
        }
    }
    void SortBy(string parametr)
    {
        if (parametr == "По площади")
            countrys = countrys.OrderBy(c => c.space).ToList();
        if (parametr == "По площади по убыванию")
            countrys = countrys.OrderByDescending(c => c.space).ToList();
        if (parametr == "По ВВП")
            countrys = countrys.OrderBy(c => c.gdp).ToList();
        if (parametr == "По ВВП по убыванию")
            countrys = countrys.OrderByDescending(c => c.gdp).ToList();
        if (parametr == "По населению")
            countrys = countrys.OrderBy(c => c.population).ToList();
        if (parametr == "По населению по убыванию")
            countrys = countrys.OrderByDescending(c => c.population).ToList();

        ButtonListOnClick();
    }

    void SortButtonOnClick(string parametr, Button btn)
    {
        Image imageArrow = btn.transform.GetChild(1).GetComponent<Image>();        
        if (imageArrow.transform.rotation.eulerAngles.z == 0)
        {             
            imageArrow.transform.rotation = Quaternion.Euler(imageArrow.transform.rotation.eulerAngles.x, imageArrow.transform.rotation.eulerAngles.y, imageArrow.transform.rotation.eulerAngles.z + 180);

            SortBy(parametr);
        }
        else
        {
            imageArrow.transform.rotation = Quaternion.Euler(imageArrow.transform.rotation.eulerAngles.x, imageArrow.transform.rotation.eulerAngles.y, imageArrow.transform.rotation.eulerAngles.z - 180);

            SortBy(parametr + " по убыванию");
        }
    }     

    void ButtonListOnClick()
    {
        CountCountryPanels = 1;
        for (int i = 0; i < CountryPanels.Count; i++)
        {
            Destroy(CountryPanels[i]);
        }
        CountryPanels.Clear();        

        windowSelectedCountrys.SetActive(true);        
        for (int i = 0; i < countrys.Count; i++)
        {
            if (countrys[i].selected)
            {
                GameObject countryPanel = Instantiate(prefabCountryPanel, windowSelectedCountrys.transform);
                RectTransform TrCountryPanel = countryPanel.GetComponent<RectTransform>();
                if (countryPanel.transform.childCount > 0)
                {
                    
                    int indent = 55;
                    //  countryPanel.transform.position -= new Vector3(0, indent * CountCountryPanels,0);
                    TrCountryPanel.offsetMax -= new Vector2(0, indent * CountCountryPanels);
                    TrCountryPanel.offsetMin -= new Vector2(0, indent * CountCountryPanels);
                    countryPanel.transform.GetChild(0).GetComponent<Text>().text = countrys[i].name;
                    countryPanel.transform.GetChild(1).GetComponent<Text>().text = countrys[i].space.ToString();
                    countryPanel.transform.GetChild(2).GetComponent<Text>().text = countrys[i].population.ToString();
                    countryPanel.transform.GetChild(3).GetComponent<Text>().text = countrys[i].gdp.ToString();

                    CountCountryPanels++;
                }
                CountryPanels.Add(countryPanel);
            }
        }
    }

    void ButtonBackOnClick() {        
        windowSelectedCountrys.SetActive(false);
    }

    void ButtonClearOnClick()
    {
        numberSelectedCountrys = 0;
        for (int i = 0; i < countrys.Count; i++)
        {
            if (countrys[i].selected)
                countrys[i].selected = false;
        }
        for (int i = 0; i < ActiveMarkers.Count; i++)
        {
            if (ActiveMarkers[i].transform.childCount > 0)
            {
                ActiveMarkers[i].transform.GetChild(0).gameObject.SetActive(true);
                ActiveMarkers[i].transform.GetChild(1).gameObject.SetActive(false);
                ActiveMarkers[i].transform.GetChild(2).gameObject.SetActive(false);
                ActiveMarkers[i].GetComponent<Marker>().selected = false;
            }
        }
        ActiveMarkers.Clear();
        ClearButton.gameObject.SetActive(false);
        selectedCountrysPanel.SetActive(false);
        cam.selectedMode = false;
    }
}
