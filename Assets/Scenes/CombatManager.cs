using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public enum CombatState
    {
        Inactive,
        Start,
        PlayerTurn,
        EnemyTurn,
        BetweenTurns
    }
    public CombatState combatState { get; private set; }

    public float ringWidth;
    public GameObject ring;

    public GameObject combatUI;
    public List<GameObject> combatants { get; private set; }

    //player combat mechanics
    private GameObject player;
    private Vector2 turnStartPosition;
    public float maxCombatMoveDistance;//temporary variable. replace with character movement points


    // Start is called before the first frame update
    void Start()
    {
        combatState = CombatState.Inactive;
        transform.localScale = new Vector3(ringWidth, ringWidth, 1);
        ring.SetActive(false);
        combatUI.SetActive(false);
        combatants = new List<GameObject>();
        

        InvokeRepeating("CombatUpdate", 0, 0.05f);
    }

    // Update is called once per frame
    private void CombatUpdate()
    {
        switch (combatState)
        {
            case CombatState.Start:
                {
                    turnStartPosition = player.transform.position;//first combatant is player. replace with search for player if needed
                    combatState = CombatState.PlayerTurn;
                    break;
                }
            case CombatState.BetweenTurns:
                {
                    break;
                }
            case CombatState.PlayerTurn:
                {
                    player.GetComponent<Controller>().canMove = (((Vector2)player.transform.position - turnStartPosition).magnitude < maxCombatMoveDistance);
                    if (Input.GetButtonDown("Fire1")) player.transform.position = turnStartPosition;
                    break;
                }
            case CombatState.EnemyTurn:
                {
                    break;
                }
        }
    }
    private void CleanCombatantsList()
    {
        if (combatants != null)
        {
            List<GameObject> cleanList = new List<GameObject>();
            for (int i = 0; i < combatants.Count; i++)
            {
                bool add = true;
                if (cleanList.Count > 0)
                {
                    for (int x = 0; x < cleanList.Count; x++)
                    {
                        if (combatants[i] == cleanList[x])
                        {
                            add = false;
                        }
                    }
                }
                if (add) cleanList.Add(combatants[i]);
            }
            if (cleanList.Count > 0) combatants = cleanList;
        }
    }

    public void StartCombat(GameObject[] enemies, GameObject triggeredEnemy)
    {
        if (combatState == CombatState.Inactive)
        {
            ring.transform.position = triggeredEnemy.transform.position - ((triggeredEnemy.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position) / 2);
            ring.SetActive(true);
            combatUI.SetActive(true);
            combatState = CombatState.Start;
            combatants.Add(GameObject.FindGameObjectWithTag("Player"));
        }

        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                combatants.Add(enemies[i]);
            }
        }
        combatants.Add(triggeredEnemy);
        CleanCombatantsList();
        player = combatants[0];
    }

    void Update()
    {
        switch (combatState)
        {
            case CombatState.PlayerTurn:
                {
                    if (Input.GetButtonDown("Fire1")) player.transform.position = turnStartPosition;
                    break;
                }

        }
    }
}
