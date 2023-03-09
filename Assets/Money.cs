using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int State;
    public int Pos;
    public float Temp;

    [Header(" --------------------------")]
    public string Animation;
    [Space(10)]
    public float TargetDist;
    public float PlayerDist;
    public float SafeDist;
    public float Reward;

    [Header(" ----------- Model -------------")]
    public int BodyCont;
    public Renderer BodyMaterial;
    public GameObject[] BodyObj;
    public GameObject[] Ragdoll;

    [Header(" _________________________ Codes _________________________ ")]
    [SerializeField] private Scenario Sce;

    void Start()
    {
        Sce = GameObject.Find("Scenario").GetComponent<Scenario>();

        Reward = Random.Range(5, 10);
        Temp = 1;
        Pos = -1;
        Animation = "Idle";
        BodyMaterial.material.color = Color.grey;
        this.GetComponent<Animation>().enabled = true;
        BodyCont = Random.Range(0, (BodyObj.Length - 1));

        for (int i = 0; i < Ragdoll.Length; i++)
        {
            Ragdoll[i].tag = "Respawn";

            if (Ragdoll[i].GetComponent<SphereCollider>()) { Ragdoll[i].GetComponent<SphereCollider>().enabled = false; }
            if (Ragdoll[i].GetComponent<BoxCollider>()) { Ragdoll[i].GetComponent<BoxCollider>().enabled = false; }
            if (Ragdoll[i].GetComponent<CapsuleCollider>()) { Ragdoll[i].GetComponent<CapsuleCollider>().enabled = false; }
        } // Ragdoll *

        for (int i = 0; i < BodyObj.Length; i++) { BodyObj[i].SetActive(false); }
    } // Start

    void Update()
    {
        Sce.UnitQueue++;
        PlayerDist = Vector3.Distance(this.transform.position, Sce.PlayerObj.transform.position);

        FunctionUnitLimits();        
        Sce.FunctionSpace(this.gameObject, 5);

        if (State <= 4) { FunctionUnitStates(); }
        if (State >= 4) { FunctionUnitDefeated(); }

        // ---------------
        if ((State == 3 || State == 4) && Pos == 11)
        {
            Debug.Log("Error: multiple units go in place [11]");
            FunctionFindPlace();
        } // BUG
    } // Update

    // ------------------------------------------
    public void FunctionUnitLimits()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
        this.GetComponent<Animation>().Play(Animation);
        this.GetComponent<Rigidbody>().isKinematic = false;
      
        if (Pos <= 0) { Pos = 0; }
        if (Temp <= 0) { Temp = 0; }
        if (Temp > 0) { Temp -= Time.deltaTime; }

        if (Reward >= 50 && State <= 4)
        {
            Reward = 50;
            BodyMaterial.material.color = Color.cyan;
        }
    } // FunctionLimits

    public void FunctionUnitStates()
    {
        if (State == 0 && Pos >= 0)
        {
            int AA = Random.Range(0, (Sce.PlaceWait.Length - 2));

            if (!Sce.PlacePoint[AA])
            {
                Pos = AA;
                if (Pos >= (Sce.PlaceWait.Length - 1)) { Pos = (Sce.PlaceWait.Length - 1); }

                Temp = Random.Range(5, 15);
                Animation = "Idle";
                BodyObj[BodyCont].SetActive(true);

                State = 1;
            } // Ready *
        } // Born *

        if (State == 1)
        {
            Animation = "Walk";
            TargetDist = Vector3.Distance(this.transform.position, Sce.PlaceWait[Pos].transform.position);
            
            this.transform.LookAt(Sce.PlaceWait[Pos].transform.position, Vector3.left);
            this.transform.Translate(0, 0, 5 * Time.deltaTime);

            // ----------------
            if (TargetDist <= 1.25f) { State = 2; }
            if (Pos >= (Sce.PlaceWait.Length - 1)) { Pos = (Sce.PlaceWait.Length - 1); }
        } // Walk - Wait *

        if (State == 2) 
        {
            if (Pos >= (Sce.PlaceWait.Length - 1)) { Pos = (Sce.PlaceWait.Length - 1); }

            Animation = "Idle";
            Temp -= Time.deltaTime;
            Sce.PlacePoint[Pos] = true;
            this.transform.position = Sce.PlaceWait[Pos].transform.position;

            if (Temp <= 0) { FunctionFindPlace(); } // Seek Place
        } // Idle - Wait *

        if (State == 3)
        {
            Animation = "Walk";
            Sce.ExercisePoint[Pos] = true;
            TargetDist = Vector3.Distance(this.transform.position, Sce.PlaceExecise[Pos].transform.position);

            this.transform.LookAt(Sce.PlaceExecise[Pos].transform.position, Vector3.left);
            this.transform.Translate(0, 0, 5 * Time.deltaTime);

            // ----------------
            if (TargetDist <= 1.25f) { State = 4; }
            if (Pos >= (Sce.PlaceExecise.Length - 1)) { Pos = (Sce.PlaceExecise.Length - 1); }
        } // Walk - Treadmill *

        if (State == 4)
        {
            if (Pos >= (Sce.PlaceExecise.Length - 1)) { Pos = (Sce.PlaceExecise.Length - 1); }
            if (Reward < 50) { Reward += 1.25f * Time.deltaTime; }

            Temp = 1;
            Animation = "Run";
            Sce.ExercisePoint[Pos] = true;            
            this.transform.position = Sce.PlaceExecise[Pos].transform.position;
        } // Idle - Treadmill *

        this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
    } // FunctionUnitStates

    public void FunctionUnitDefeated()
    {
        SafeDist = 5 + (Sce.PlayerObj.GetComponent<Player>().Level / 100) + (Sce.PlayerObj.GetComponent<Player>().Money / 1000);
        Ragdoll[0].transform.localPosition = new Vector3(0, 0.35f, 0);

        if (State <= 4 && Temp <= 1 && PlayerDist <= 1)
        {
            for (int i = 0; i < BodyObj.Length; i++) { BodyObj[i].SetActive(false); }
            for (int i = 0; i < Sce.PlacePoint.Length; i++) { Sce.PlacePoint[i] = false; }
            for (int i = 0; i < Sce.ExercisePoint.Length; i++) { Sce.ExercisePoint[i] = false; }
            for (int i = 0; i < Ragdoll.Length; i++)
            {
                Ragdoll[i].tag = "Respawn";

                if (Ragdoll[i].GetComponent<Rigidbody>()) { Ragdoll[i].GetComponent<Rigidbody>().drag = 1; }
                if (Ragdoll[i].GetComponent<SphereCollider>()) { Ragdoll[i].GetComponent<SphereCollider>().enabled = true; }
                if (Ragdoll[i].GetComponent<BoxCollider>()) { Ragdoll[i].GetComponent<BoxCollider>().enabled = true; }
                if (Ragdoll[i].GetComponent<CapsuleCollider>()) { Ragdoll[i].GetComponent<CapsuleCollider>().enabled = true; }
            } // Ragdoll *

            BodyMaterial.material.color = Color.white;
            this.GetComponent<CapsuleCollider>().enabled = false;
            Sce.PlayerObj.GetComponent<Player>().Push = 0.5f;
            Pos = 0;
            State = 5;
            Temp = 1;
        } // Hit

        // ------------------------------------------
        if (State == 5 || (State >= 6 && (PlayerDist > SafeDist)))
        {
            State = 5;
            this.GetComponent<Animation>().enabled = false;

            if (PlayerDist <= 1.25f && Temp <= 0) { State = 6; }
        } // Knocked down *

        if (State >= 6 && PlayerDist <= SafeDist && (Sce.PlayerObj.GetComponent<Player>().Load < (Sce.PlayerObj.GetComponent<Player>().Level + 1)))
        {
            Debug.Log("Aplly Inertia in Units");
            
            State = 6;
            this.GetComponent<Animation>().enabled = false;
            TargetDist = Vector3.Distance(this.transform.position, Sce.SafeObj.transform.position);

            Pos = Sce.PlayerObj.GetComponent<Player>().Load;
            Sce.PlayerObj.GetComponent<Player>().Load++;
            this.transform.position = Sce.PlayerObj.GetComponent<Player>().Pile[Pos].transform.position;

            // ----------------
            if (TargetDist <= 1 && Sce.SafeTemp <= 0)
            {
                Sce.SafeTemp = 0.75f;
                Sce.PlayerObj.GetComponent<Player>().Money += (int)Reward;
                Destroy(this.gameObject);
            }
        } // Being Loaded *
    } // FunctionUnitDefeated

    public void FunctionFindPlace()
    {
        int AA = Random.Range(0, (Sce.PlaceExecise.Length - 2));

        if (!Sce.ExercisePoint[AA])
        {
            Pos = AA;
            if (Pos >= (Sce.PlacePoint.Length - 1)) { Pos = (Sce.PlacePoint.Length - 1); }

            Sce.PlacePoint[Pos] = false;
            Animation = "Idle";
            State = 3;
        } // Ready
    } //FunctionFindPlace
} // End