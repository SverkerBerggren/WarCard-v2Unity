
using RuleManager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; 
public class CanvasUiScript : MonoBehaviour
{
    public Image topFrame; 
    public Sprite firstPlayerTopFrame;
    public Sprite secondPlayerTopFrame;
    private bool isFirstPlayerTopFrame = true;

    public GameObject unitCard;
    public GameObject unitActions;
    public UnitActions unitAbilities;
    public GameObject genericAbilityButton; 

    public MainUI mainUI;

    private List<GameObject> buttonDestroyList = new List<GameObject>();

    [SerializeField]
    private ErrorMessageScript errorMessageScript;


    [SerializeField]
    private TextMeshProUGUI reactionText;

    public TextMeshProUGUI Player1Score;
    public TextMeshProUGUI Player2Score;

    public static CanvasUiScript instance;
    // Start is called before the first frame update
    void Start()
    {
        reactionText.gameObject.SetActive(false);

        mainUI = FindObjectOfType<MainUI>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    Sprite p_SpriteFromVisual(ResourceManager.Visual VisualToConvert)
    {
        Sprite ReturnValue = null;
        if(VisualToConvert is ResourceManager.Visual_Image)
        {
            ReturnValue = ((ResourceManager.Visual_Image)VisualToConvert).Sprite;
        }
        return ReturnValue;
    }
    public void createUnitCard(UnitInfo unitInfo)
    {
        RuleManager.RuleManager ruleManager = mainUI.ruleManager; 
        


        unitCard.SetActive(true);

        unitActions.SetActive(true);

        UnitCardScript unitCardInformation = unitCard.GetComponent<UnitCardScript>();

        unitCardInformation.DamageText.text = unitInfo.Stats.Damage.ToString();
        unitCardInformation.HpText.text = unitInfo.Stats.HP.ToString();
        unitCardInformation.ActivationCostText.text = unitInfo.Stats.ActivationCost.ToString();
        unitCardInformation.MovementText.text = unitInfo.Stats.Movement.ToString();
        unitCardInformation.RangeText.text = unitInfo.Stats.Range.ToString();
        unitCardInformation.ObjectiveControlText.text = unitInfo.Stats.ObjectiveControll.ToString();

        unitCardInformation.gameObject.GetComponent<Image>().sprite = p_SpriteFromVisual(mainUI.GetUnitUIInfo(unitInfo).UIInfo.DownAnimation.VisualInfo);
        if ((ruleManager.GetUnitInfo(unitInfo.UnitID).Flags & RuleManager.UnitFlags.HasMoved) != 0)
        {
            GameObject.Find("MoveButton").GetComponent<Button>().interactable = false;
        }
        else
        {
            GameObject.Find("MoveButton").GetComponent<Button>().interactable = true;
        }
        if ((ruleManager.GetUnitInfo(unitInfo.UnitID).Flags & RuleManager.UnitFlags.HasAttacked) != 0)
        {
            GameObject.Find("AttackButton").GetComponent<Button>().interactable = false;
        }
        else
        {
            GameObject.Find("AttackButton").GetComponent<Button>().interactable = true;
        }


        int padding = 150;
        int ogPadding = padding;
        int index = 0;

        //print(padding);
        //   print("hur manga barn innan destroy " + GameObject.Find("UnitActions").transform.childCount);
        DestroyButtons();
        //   print("hur manga barn efter destroy " + GameObject.Find("UnitActions").transform.childCount);
        //    GameObject.Find("UnitActions").GetComponent<UnitActions>().clearAbilityButtons();
        //foreach (RuleManager.Ability ability in unitInfo.Abilities)

        

        for (int i = 0; i < unitInfo.Abilities.Count; i++)
        {
            RuleManager.Ability ability = unitInfo.Abilities[i];
            //  insta
            GameObject attackButton = GameObject.Find("AttackButton");
            //      genericAbilityButton.SetActive(true);



            GameObject newButton = Instantiate(genericAbilityButton, new Vector3(attackButton.transform.position.x + padding, -390, 0), new Quaternion());
            padding += ogPadding;

            newButton.transform.SetParent(GameObject.Find("UnitAbilitys").transform);
            newButton.transform.position = new Vector3(attackButton.transform.position.x + padding, 0, 0);

            AbilityButton abilityButton = newButton.GetComponent<AbilityButton>();

            abilityButton.abilityIndex = index;

            abilityButton.abilityFlavour = ability.GetFlavourText();

            abilityButton.abilityDescription = ability.GetDescription();

            print("vad returnerar getName " + ability.GetName());

            abilityButton.abilityName = ability.GetName();

            abilityButton.playerIndex = unitInfo.PlayerIndex;

            abilityButton.unitInfo = unitInfo;

            var AssociatedUnit =mainUI.GetUnitUIInfo(unitInfo);
            abilityButton.SetClickable(unitInfo.AbilityActivationCount[i] == 0);

            if (AssociatedUnit.TotalAbilities.ContainsKey(i))
            {
                if(AssociatedUnit.TotalAbilities[i].Icon != null)
                {
                    newButton.GetComponent<Image>().sprite = p_SpriteFromVisual(AssociatedUnit.TotalAbilities[i].Icon.VisualInfo);
                }
            }

            if (ability is RuleManager.Ability_Activated)
            {
                RuleManager.Ability_Activated activatedAbility = (RuleManager.Ability_Activated)ability;

                abilityButton.activatedAbility = true;


                if (activatedAbility.ActivationTargets is RuleManager.TargetInfo_List)
                {
                    RuleManager.TargetInfo_List listOfTargets = (RuleManager.TargetInfo_List)activatedAbility.ActivationTargets;

                    abilityButton.whichTargets = listOfTargets.Targets;
                }

                //    abilityButton.whichTargets = activatedAbility.ActivationTargets.;



            }
            if (ruleManager.GetUnitInfo(unitInfo.UnitID).Abilities[i] is RuleManager.Ability_Activated)
            {
                newButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                newButton.GetComponent<Button>().interactable = false;
            }


            buttonDestroyList.Add(newButton);

            index += 1;
            unitAbilities.sortChildren();
        }
    }

    public void DisableUnitCard()
    {

        unitCard.SetActive(false);

        unitActions.SetActive(false);
    }



    public void DestroyButtons()
    {
        foreach (GameObject obj in buttonDestroyList)
        {
            obj.transform.SetParent(null);
            Destroy(obj);

        }
        buttonDestroyList.Clear();
    }
    public void changeTopFrame(int PlayerTurn)
    {
        if(PlayerTurn == 0)
        {
            topFrame.sprite = firstPlayerTopFrame;
        }
        else if(PlayerTurn == 1)
        {
            topFrame.sprite = secondPlayerTopFrame;
        }
    }

    public IEnumerator changeReactionText(bool active, string text)
    {
        reactionText.gameObject.SetActive(active);

        reactionText.text = text; 

        yield return new WaitForSeconds(0.1f);
    }

    public void errorMessage(string message)
    {
        errorMessageScript.timer = errorMessageScript.originalTimer;
        errorMessageScript.errorMessageTextMesh.text = message;
        errorMessageScript.gameObject.SetActive(true);

    }

    public void ChangePlayerScore(int PlayerIndex, int newScore)
    {

        if (PlayerIndex == 0)
        {
            Player1Score.text = "Player 1 Score: " + newScore;
        }
        else
        {
            Player2Score.text = "Player 2 Score: " + newScore;
        }
    }

    private void CreateStatusEffect()
    {

    }
}
