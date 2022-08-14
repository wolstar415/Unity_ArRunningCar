using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;



public class ArManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_AnchorPrefab;

    public GameObject AnchorPrefab
    {
        get => m_AnchorPrefab;
        set => m_AnchorPrefab = value;
        
        
    }
    

    void Awake()
    {
        
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_AnchorManager = GetComponent<ARAnchorManager>();
        m_PlaneManager = GetComponent<ARPlaneManager>();
        m_AnchorPoints = new List<ARAnchor>();
    }
    // Update is called once per frame
    void Update()
    {
        //PlacePrefab();
    }
    
    public ARSessionOrigin arOrigin;
    List<ARRaycastHit> originHits = new List<ARRaycastHit>();

    public void PlaceOrigin()
    {
        m_RaycastManager.Raycast(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f), originHits, TrackableType.Planes);
        if (originHits.Count > 0)
        {
            Pose hitpose = originHits[0].pose;

            // 괄호안에 들어가는 것이 원점이 된다
            arOrigin.MakeContentAppearAt(arOrigin.transform, hitpose.position, Quaternion.Inverse(hitpose.rotation));
        }
    }

    public void PlaneOff()
    {
        m_PlaneManager.enabled = false;
        foreach (var plane in m_PlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    private void PlacePrefab()
    {
        
        if (Input.touchCount == 0)
            return;

        if (IsPointerOverUIObject())
        {
            return;
        }

        if (GameManager.inst.startBool == true) return;
        var touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began)
            return;

        if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.Planes))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            Pose hitPose = s_Hits[0].pose;
            
            var hitTrackableId = s_Hits[0].trackableId;
            var hitPlane = m_PlaneManager.GetPlane(hitTrackableId);

            // This attaches an anchor to the area on the plane corresponding to the raycast hit,
            // and afterwards instantiates an instance of your chosen prefab at that point.
            // This prefab instance is parented to the anchor to make sure the position of the prefab is consistent
            // with the anchor, since an anchor attached to an ARPlane will be updated automatically by the ARAnchorManager as the ARPlane's exact position is refined.
            var anchor = m_AnchorManager.AttachAnchor(hitPlane, hitPose);
            //var a=Instantiate(m_AnchorPrefab, anchor.transform);
            Quaternion a = hitPose.rotation;
            
                a.y = 0;



            if (!GameManager.inst.playerOb.activeSelf)
            {
                GameManager.inst.playerOb.SetActive(true);
            }
            GameManager.inst.playerOb.transform.position = hitPose.position+new Vector3(0,-3,0);
            GameManager.inst.playerOb.transform.rotation = a;
            
            //a.transform.position = a.transform.position + new Vector3(0, 0, 0);
            //GameManager.inst.startBool = true;
            //GameManager.inst.playerOb = a.transform.GetChild(0).gameObject;
            if (anchor == null)
            {
                Debug.Log("Error creating anchor.");
            }
            else
            {
                // Stores the anchor so that it may be removed later.
                m_AnchorPoints.Add(anchor);
            }
        }
    }
    
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    List<ARAnchor> m_AnchorPoints;

    public ARRaycastManager m_RaycastManager;

    public ARAnchorManager m_AnchorManager;

    public ARPlaneManager m_PlaneManager;
}
