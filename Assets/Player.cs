using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Action;
    public float Temp;
    public float Push;
    public string Animation;
    [Space(10)]
    public GameObject[] Body;
    public GameObject[] Pile;
    public Renderer BodyMaterial;

    [Header(" --------------------------")]
    public int Level;
    public int Load;
    public int Money;
    public int Experience;

    [Header(" _________________________ Codes _________________________ ")]
    [SerializeField] private Scenario Sce;

    void Start() 
    {
        Sce = GameObject.Find("Scenario").GetComponent<Scenario>();

        Animation = "Idle";
        BodyMaterial.material.color = Color.white;

        for (int i = 0; i < Body.Length; i++) { Body[i].SetActive(false); }
        for (int i = 0; i < Pile.Length; i++) { Pile[i].layer = 1; }
    } // Start

    void Update()
    {
        Sce.PlayerObj = this.gameObject;

        FunctionPlayerLimits();
        FunctionPlayerControls();
        Sce.FunctionSpace(this.gameObject, 5);
    } // Update

    // ------------------------------------------
    public void FunctionPlayerLimits()
    {
        Load = 0;
        this.transform.localScale = new Vector3(1, 1, 1);

        this.GetComponent<Animation>().enabled = true;
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<CapsuleCollider>().isTrigger = true;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationZ;

        if (Temp > 0) { Temp -= Time.deltaTime; }

        if (Level >= (Body.Length - 1)) { Level = (Body.Length - 1); Experience = 0; } // Level limit

        if (Experience <= 0) { Experience = 0; }
        if (Money <= 0) { Money = 0; }
        if (Money >= 9999999) { Money = 9999999; }
    } // FunctionPlayerLimits

    public void FunctionPlayerControls()
    {
        Body[Level].SetActive(true);

        if (Push > 0) { Animation = "Victory"; Push -= Time.deltaTime; }

        if (Temp <= 0 || Action <= 0)
        {
            Temp = 0; 
            Action = 0;
            if (Push <= 0) { Animation = "Idle"; }           
        } // Default *

        if (Action == 1 || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.RightArrow))
        {
            if (Push <= 0) { Animation = "Run"; }

            Action = 1;
            this.transform.Translate(0, 0, 5 * Time.deltaTime);
        } // Move *

        if (Action == 2 || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (Push <= 0) { Animation = "Run"; }

            Action = 2;
            this.transform.Rotate(0, -150 * Time.deltaTime, 0);
        } // Left *

        if (Action == 3 || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (Push <= 0) { Animation = "Run"; }

            Action = 3;            
            this.transform.Rotate(0, 150 * Time.deltaTime, 0);
        } // Right *

        this.GetComponent<Animation>().Play(Animation);
    } // FunctionPlayerControls
} // End