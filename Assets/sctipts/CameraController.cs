using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 startPosition;
    Camera cam;
    Marker newMarker;
    Marker selectedMarker;

    public bool selectedMode;

    float time;

    float sensitivity = 0.2f;
    Vector2 f0start;
    Vector2 f1start;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.touchCount < 2)
        {
            f0start = Vector2.zero;
            f1start = Vector2.zero;
        }
        if (Input.touchCount == 2) Zoom();
        else
        if (Input.GetMouseButtonDown(0))
        {
            time = 0;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (selectedMode)
                {
                    newMarker = hit.transform.GetComponent<Marker>();
                    if (newMarker)
                    {
                        newMarker.SelectCountry();
                    }
                }
                else
                {
                    newMarker = hit.transform.GetComponent<Marker>();                   
                    if (newMarker)
                    {
                        if (selectedMarker && selectedMarker != newMarker)
                            if (selectedMarker.transform.childCount > 0)
                            {
                                selectedMarker.transform.GetChild(0).gameObject.SetActive(true);
                                selectedMarker.transform.GetChild(1).gameObject.SetActive(false);
                            }
                        selectedMarker = newMarker;
                        newMarker.ShowModel();
                    }                    
                }
                startPosition = hit.point;
            }
        }
        else if (Input.GetMouseButton(0))
        {            
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (newMarker)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.GetComponent<Marker>())
                    {
                        time += Time.deltaTime;
                        if (time > 1)
                        {
                            newMarker.SelectCountry();
                            selectedMode = true;
                            time = 0;
                        }                        
                    }
                }
            }
            if (!newMarker)
            {
                Vector3 pos = new Vector3();
                if (Physics.Raycast(ray, out hit))
                {
                    pos = hit.point - startPosition;
                }

                transform.position = new Vector3(Mathf.Clamp(transform.position.x - pos.x, -15f, 950f), transform.position.y, Mathf.Clamp(transform.position.z - pos.z, 20f, 580f));
            }            
        }
    }


    void Zoom()
    {
        if (f0start == Vector2.zero && f1start == Vector2.zero)
        {
            f0start = Input.GetTouch(0).position;
            f1start = Input.GetTouch(1).position;
        }

        Vector2 f0position = Input.GetTouch(0).position;
        Vector2 f1position = Input.GetTouch(1).position;

        float dir = Mathf.Sign(Vector2.Distance(f1start, f0start) - Vector2.Distance(f0position, f1position));
        if (transform.position.y > 1 && transform.position.y < 60)
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, dir * sensitivity * Time.deltaTime * Vector3.Distance(f0position, f1position));
        else
        {
            if (transform.position.y < 1)
            {
                transform.position = new Vector3(transform.position.x, 1.2f, transform.position.z);
            }
            if(transform.position.y > 60)
                transform.position = new Vector3(transform.position.x, 59.8f, transform.position.z);
        }

    }
}
