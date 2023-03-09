using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public RectTransform XpObj;
    public TextMeshProUGUI XpTxt;
    public RectTransform MoneyTxt;
    public GameObject LevelObj;
    public TextMeshProUGUI LevelTxt;

    [Header(" _________________________ Codes _________________________ ")]
    [SerializeField] private Scenario Sce;

    void Start() { Sce = GameObject.Find("Scenario").GetComponent<Scenario>(); } // Start

    void Update()
    {
        FunctionInformationsHUD(Sce.PlayerObj.GetComponent<Player>().Money);

        if (Sce.SafeDist <= 1.5f) { FunctionLevelUp(); }
    } // Update

    // ------------------------------------------
    public void FunctionInformationsHUD(int Num)
    {
        XpObj.GetComponent<Scrollbar>().size = (Sce.PlayerObj.GetComponent<Player>().Experience / 100);
        XpTxt.text = "Level: " + Sce.PlayerObj.GetComponent<Player>().Level;

        MoneyTxt.GetComponent<TextMeshProUGUI>().text = "$ " + Num;

        LevelObj.gameObject.SetActive(false);
        LevelTxt.gameObject.SetActive(false);
        LevelTxt.text = "Level Up: $100 \n    Press [Space]";
    } // FunctionInformations

    public void FunctionLevelUp()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Sce.PlayerObj.GetComponent<Player>().Money >= 100)
        {
            for (int i = 0; i < Sce.PlayerObj.GetComponent<Player>().Body.Length; i++) { Sce.PlayerObj.GetComponent<Player>().Body[i].SetActive(false); }

            Sce.PlayerObj.GetComponent<Player>().Level++;
            Sce.PlayerObj.GetComponent<Player>().Money -= 100;

            // Player Color -------------------------------------------
            int C = Random.Range(1, 5);

            if (C <= 1) { Sce.PlayerObj.GetComponent<Player>().BodyMaterial.material.color = Color.blue; }
            if (C == 2) { Sce.PlayerObj.GetComponent<Player>().BodyMaterial.material.color = Color.red; }
            if (C == 3) { Sce.PlayerObj.GetComponent<Player>().BodyMaterial.material.color = Color.green; }
            if (C >= 4) { Sce.PlayerObj.GetComponent<Player>().BodyMaterial.material.color = Color.cyan; }
        } // Comando *

        LevelObj.gameObject.SetActive(true);
        LevelTxt.gameObject.SetActive(true);
    } // FunctionLevelUp
} // End