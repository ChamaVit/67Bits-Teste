using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scenario : MonoBehaviour
{
    public GameObject CameraObj;
    public GameObject PlayerObj;

    [Header(" ----------- Units ---------------")]
    public float UnitTemp;
    public int UnitQueue;
    public GameObject UnitObj;
    public GameObject EnterObj;

    [Header(" ------------ Safe ---------------")]
    public float SafeDist;
    public float SafeTemp;
    public GameObject SafeObj;

    [Header(" ----------- Locals ---------------")]
    public bool[] ExercisePoint;
    public GameObject[] PlaceExecise;
    [Space(10)]
    public bool[] PlacePoint;
    public GameObject[] PlaceWait;

    void Start()  { UnitTemp = 5; } // Start

    void Update()
    {
        this.transform.position = new Vector3(0, 0, 0);

        FunctionLimits();
        FunctionCreateUnits();

        FunctionSpace(CameraObj.gameObject, 6);
    } // Update

    // ------------------------------------------
    public void FunctionLimits()
    {
        if (PlayerObj == null) { Application.Quit(); }

        if (SafeTemp > 0) { SafeTemp -= Time.deltaTime; }
        if (SafeTemp <= 0) { SafeTemp = 0; }

        SafeObj.transform.position = new Vector3(0, 0.05f, 0);
        SafeDist = Vector3.Distance(SafeObj.transform.position, PlayerObj.transform.position);

        CameraObj.transform.LookAt(PlayerObj.transform.position, CameraObj.transform.position);
        CameraObj.transform.eulerAngles = new Vector3(CameraObj.transform.eulerAngles.x, CameraObj.transform.eulerAngles.y, 0);
    } // FunctionLimits

    public void FunctionCreateUnits()
    {
        int Cont = 0;
        UnitQueue = 0;

        for (int i = 0; i < PlacePoint.Length; i++) { if (!PlacePoint[i]) { Cont++; } }

        // -------------
        if (UnitTemp > 0) { UnitTemp -= Time.deltaTime; }

        if (UnitTemp <= 0)
        {
            if (UnitQueue < Cont) { Instantiate(UnitObj, EnterObj.transform.position, Quaternion.identity); }            
            UnitTemp = 5;
        }
    } // FunctionCreateUnits

    public void FunctionSpace(GameObject Obj, float LimitY)
    {
        float PosX = Obj.transform.position.x;
        float PosY = Obj.transform.position.y;
        float PosZ = Obj.transform.position.z;

        if (PosX >= 10) { Obj.transform.position = new Vector3(10, Obj.transform.position.y, Obj.transform.position.z); }
        if (PosX <= -10) { Obj.transform.position = new Vector3(-10, Obj.transform.position.y, Obj.transform.position.z); }

        if (PosY >= LimitY) { Obj.transform.position = new Vector3(Obj.transform.position.x, LimitY, Obj.transform.position.z); }
        if (PosY <= 0) { Obj.transform.position = new Vector3(Obj.transform.position.x, 0, Obj.transform.position.z); }

        if (PosZ >= 10) { Obj.transform.position = new Vector3(Obj.transform.position.x, Obj.transform.position.y, 10); }
        if (PosZ <= -10) { Obj.transform.position = new Vector3(Obj.transform.position.x, Obj.transform.position.y, -10); }
    } // FunctionSpace
} // End