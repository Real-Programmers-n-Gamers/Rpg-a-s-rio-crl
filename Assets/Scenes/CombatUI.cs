using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    public GameObject turnLine;
    public GameObject combatManager;
    private CombatManager combatManagerScript;
    private List<GameObject> combatants;
    private List<GameObject> turnLineCombatants;

    // Start is called before the first frame update
    void Start()
    {
        combatManagerScript = combatManager.GetComponent<CombatManager>();
        combatants = new List<GameObject>();
        turnLineCombatants = new List<GameObject>();

        InvokeRepeating("UpdateCombatUI", 0.02f, 0.05f);
    }

    private void SetCombatantsTurnLine()
    {
        if (turnLine.transform.childCount > 0)
        {
            for (int i = 0; i < turnLine.transform.childCount; i++)
            {
                Destroy(turnLine.transform.GetChild(i).gameObject);
            }
        }

        combatants = combatManagerScript.combatants;
        turnLineCombatants.Clear();
        if (combatants.Count > 0)
        {
            for (int i = 0; i < combatants.Count; i++)
            {
                turnLineCombatants.Add(new GameObject());
                turnLineCombatants[i].transform.SetParent(turnLine.transform);
                turnLineCombatants[i].AddComponent<Image>();
                if (i == 0) turnLineCombatants[i].GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>().sprite;
                else turnLineCombatants[i].GetComponent<Image>().sprite = combatants[i].GetComponent<EnemyController>().sprite;
                turnLineCombatants[i].GetComponent<Image>().preserveAspect = true;
                turnLineCombatants[i].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
    }

    // Update is called once per frame
    private void UpdateCombatUI()
    {
        if (combatants.Count != combatManagerScript.combatants.Count) SetCombatantsTurnLine();
    }
}
