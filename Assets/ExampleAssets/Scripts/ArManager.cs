using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ArManager : MonoBehaviour
{
    public ARRaycastManager m_RaycastManager;
    public ARPlaneManager m_PlaneManager;

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_PlaneManager = GetComponent<ARPlaneManager>();
    }

    public ARSessionOrigin arOrigin;
    List<ARRaycastHit> originHits = new List<ARRaycastHit>();

    public void PlaceOrigin()
    {
        m_RaycastManager.Raycast(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f), originHits,
            TrackableType.Planes);
        //Raycast로 바닥을 쏩니다.
        if (originHits.Count > 0)
        {
            Pose hitpose = originHits[0].pose;
            //Raycasy에 닿은 첫번째의 위치를 가져옵니다.
            arOrigin.MakeContentAppearAt(arOrigin.transform, hitpose.position, Quaternion.Inverse(hitpose.rotation));
            //ARARSessionOrigin의 포지션을 바꿉니다.
        }
    }

    public void PlaneOff()
        //바닥오브젝트 비활성화
    {
        m_PlaneManager.enabled = false;
        foreach (var plane in m_PlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }
}