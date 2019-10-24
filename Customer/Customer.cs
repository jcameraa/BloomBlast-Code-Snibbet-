using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum Difficulty
{
    Custom,
    Easy,
    Medium,
    Hard
}

/// <summary>
/// The manager for each individul customer that appears. This script handles
/// each customer indivisually.
/// </summary>
public class Customer : MonoBehaviour
{
    /// <summary>
    /// The index of the customer in the customer manager list
    /// </summary>
    public int customerIndex;
    [Space]

    #region Variable
    /// <summary>
    /// The type of plant the customer wants
    /// </summary>
    public Dot DotWanted;

    /// <summary>
    /// The amount of plants this customer wants
    /// </summary>
    public float NumWanted;

    // The animatior attached to the current
    // character shown 
    public Animator charAnim;

    // The string of what type of plant
    // the charcater wants, which is assigned
    // from the tags 
    public string charWanted;

    public Difficulty LevelDifficulty;

    /// <summary>
    /// The visual representation of the NumWanted
    /// </summary>
    public Text HowManyWanted; 

    // Used to initialize the types of plants and
    // characters that will be shown on screen. 
    [SerializeField]
    public List<Image> _plantList;

    public List<GameObject> _characterList;

    protected Dictionary<string, GameObject> _plants;

    protected Dictionary<string, GameObject> _character;

    float rate = .5f;
    #endregion

    #region Difficulty Varables

    private int MinWanted;
    private int MaxWanted;

    /// <summary>
    /// Used to set up a Customer. This will give the ranges of numbers
    /// each character might be asking for based on the difficulty we select, 
    /// and then those numbers are sent to be randomly chosen for each customer
    /// </summary>
    /// <param name="difficulty">The difficultly chosen</param>
    /// <param name="dotWanted">The type of Dot the customer is asking for</param>
    /// <param name="minW">The minium amut of the Dot the customer could want</param>
    /// <param name="maxW">The maxium amount of the Dot the cusotmer could want</param>
    public void SetupCustomer(Difficulty difficulty, Dot dotWanted, int minW = 0, int maxW = 0)
    {
        DotWanted = dotWanted;
        switch (difficulty)
        {
            case Difficulty.Easy:

                MinWanted = 5;
                MaxWanted = 10;
                break;
            case Difficulty.Medium:

                MinWanted = 10;
                MaxWanted = 15;
                break;
            case Difficulty.Hard:

                MinWanted = 15;
                MaxWanted = 20;
                break;

            case Difficulty.Custom:

                MinWanted = minW;
                MaxWanted = maxW;
                break;
            default:
                break;
        }

        NumWanted = Random.Range(MinWanted, MaxWanted + 1);

        _plants = new Dictionary<string, GameObject>(_plantList.Count);
        foreach (var plant in _plantList)
        {
            plant.gameObject.SetActive(false);
            _plants.Add(plant.tag, plant.gameObject);
        }

        _character = new Dictionary<string, GameObject>(_characterList.Count);
        foreach (var chara in _characterList)
        {
            chara.gameObject.SetActive(false);
            _character.Add(chara.tag, chara.gameObject);
        }

        HowManyWanted.text = NumWanted.ToString() + "x";

    }
    #endregion

    #region Main Class Methods

    /// <summary>
    /// Displays the current plant the customer wants
    /// </summary>
    public void ShowPlant()
    {
        foreach (var plant in _plants)
        {
            plant.Value.SetActive(false);
        }
        var dotWanted = DotWanted.tag;
        _plants[dotWanted].SetActive(true);
        _plants[dotWanted].gameObject.transform.parent.gameObject.SetActive(false);
        _plants[dotWanted].gameObject.transform.parent.gameObject.SetActive(true);
    }

    

    /// <summary>
    /// Shows the customer based on what is wanted.
    /// Used with dev cheats. 
    /// </summary>
    /// <param name="charTag">The tag of the current customer</param>
    public void choseCharacter(string charTag)
    {

        Debug.Log("HOw many times :0");
        foreach (var chara in _character)
        {
            chara.Value.SetActive(false);
        }
        foreach (var plant in _plants)
        {
            plant.Value.SetActive(false);
        }
        charWanted = charTag;
        _character[charWanted].SetActive(true);
        charAnim = _character[charWanted].GetComponent<Animator>();
        var dotWanted = charTag;
        _plants[dotWanted].SetActive(true);
    }


    /// <summary>
    /// Checks if the dot destoyed is what the customer wants
    /// and if it has completed their order
    /// </summary>
    /// <param name="dot">The dot destroyed</param>
    /// <param name="numDestroyed">THe amount destroyes</param>
    /// <returns></returns>
    public bool CheckDot(Dot dot, int numDestroyed)
    {
        Debug.Log("Checking for " + DotWanted.tag);
        if (dot.gameObject.tag == DotWanted.tag)
        {
            _plants[DotWanted.tag].gameObject.GetComponentInParent<Animator>().SetTrigger("Blip");
            var before = NumWanted;
            Debug.Log("Found " + DotWanted.tag);
            NumWanted --;

            //Tweener xTween = DOTween.To(x => before = x, before, NumWanted,1f);
            //xTween.onUpdate += delegate { HowManyWanted.text = before.ToString("F0") + "x"; };
            HowManyWanted.text = NumWanted.ToString() + "x";

            Debug.Log(before + "  this is what before is" + NumWanted + "   this is what after is");
        }
        else
        {
            AnalyticsManager.Instance.LogNonObjectiveMatch(numDestroyed);
        }
        if (NumWanted <= 0)
        {
            charAnim = _character[charWanted].GetComponentInChildren<Animator>();
            ////// PUT IT HERE ////////
            return true;
        }
        //HowManyWanted.text = NumWanted.ToString() + "x";
        return false;

    }


    /// <summary>
    /// Displays the customer associated with the 
    /// plant the customer wants
    /// </summary>
    public void ShowCharacter()
    {


       foreach (var chara in _character)
        {
            chara.Value.SetActive(false);
        }

        HowManyWanted.text = NumWanted.ToString() + "x";
        charWanted = DotWanted.tag;
        _character[charWanted].SetActive(true);
        charAnim = _character[charWanted].GetComponentInChildren<Animator>();

        HowManyWanted.text = NumWanted.ToString() + "x";
        Debug.Log("Is this where that happens");
    }
    #endregion
}
