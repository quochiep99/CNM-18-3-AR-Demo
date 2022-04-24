using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacement : MonoBehaviour
{

    public GameObject arObjectToSpawn;
    public GameObject placementIndicator;
    private GameObject spawnedObject;
    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;

    // Start is called before the first frame update
    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    // Cập nhật, theo dõi chỉ báo vị trí khi di chuyển điện thoại, và sinh ra đối tượng AR
    void Update()
    {
        // Nếu không có đối tượng nào được tạo ra và chỉ báo vị trí vẫn hiện, thì khi màn hình được nhấp, sẽ sinh ra đối tượng
        if (spawnedObject == null && placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ARPlaceObject();
        }


        UpdatePlacementPose();
        UpdatePlacementIndicator();


    }
    void UpdatePlacementIndicator()
    {
        // Nếu đối tượng là null (chưa được tạo ra), thì hiện chỉ báo vị trí
        if (spawnedObject == null && placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        // Nếu đối tượng đã được tạo rồi thì ẩn chỉ báo vị trí đi
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    void UpdatePlacementPose()
    {
        // (0.5f, 0.5f) là điểm tâm chính giữa màn hình
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        // Chiếu tia xuất phát từ tâm màn hình lên mặt phẳng được xác định (gọi là TrackableTypes.Planes)        
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        // Khi ta chạm vào chỉ báo vị trí một lần (hits.Count > 0), thì chọn vị trí đó luôn
        placementPoseIsValid = hits.Count > 0; 
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;
        }
    }

    void ARPlaceObject()
    {        
        // Từ đối tượng, vị trí, phép quay góc tọa độ được tính toán ở trên, ta tiến hành tạo đối tượng ra màn hình
        spawnedObject = Instantiate(arObjectToSpawn, PlacementPose.position, PlacementPose.rotation);
    }
}


