using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// enum that stores the current state the customer is in.
/// Currently being used to handle animation states.
/// </summary>
public enum CustomerState
{
    Normal,
    Nervous
}

public class CustomerManager : MonoBehaviour
{
    #region Variables

    public CustomerState CurrentState;

    // How many dot was in the current match
    public int NumDestoryed;

    public Customer[] customerList;

    private LevelManager levelMang; 


    [HideInInspector] public bool sameCustomer;

    #endregion

    #region Initialization
    public void Init(int numOfCustomers, int moveLimit)
    {
        levelMang = GetComponentInParent<LevelManager>();
        var dotTypes = levelMang.board.GetComponent<BoardManager>().dotTypes;
        foreach (var customer in customerList)
        {
            customer.SetupCustomer(Difficulty.Easy,
            dotTypes[Random.Range(0, dotTypes.Length)]);
            customer.ShowPlant();
            customer.ShowCharacter();
            CheckIfSame(customerList);
        }

      //  AudioManager.Instance.GetFreeSource().PlayOneShot(happyCustomerSound);
    }
    #endregion

    #region Main Class Methods
    /// <summary>
    /// Checks if the group of dots that are destoryed
    /// match any of the customers currently on screen
    /// </summary>
    /// <param name="dotDestroyed"> One of the dots in the current match</param>
    /// <param name="numDestroyed"> The length of the current match </param>
    public void CheckDot(Customer[] listOfCust, Dot dotDestroyed, int numDestroyed)
    {
        //AudioManager.Instance.GetFreeSource().PlayOneShot(clickSound);
        Debug.Log("Same Customer: " + sameCustomer);
        if (!sameCustomer)
        {
            Debug.Log("Same Customer should equal false.");
            for (int i = 0; i < listOfCust.Length; i++)
            {
                if (listOfCust[i].CheckDot(dotDestroyed, numDestroyed))
                {
                    RemoveCustomer(listOfCust[i]);
                }
            }
        }
        else
        {
            Debug.Log("Same Customer should equal true.");
            if (listOfCust[0].CheckDot(dotDestroyed, numDestroyed))
            {
                RemoveCustomer(listOfCust[0]);
            }
        }  
    }

    /// <summary>
    /// Removes the customer whose order is complete.
    /// </summary>
    /// <param name="customer">The customer to remove</param>
    private void RemoveCustomer(Customer customer)
    {
        levelMang.audMang.GetComponentInChildren<UISounds>().Play("HappyCusomer");
        var dotTypes = levelMang.board.GetComponent<BoardManager>().dotTypes;
        customer.SetupCustomer(
           Difficulty.Easy, dotTypes[Random.Range(0, dotTypes.Length)]);
        customer.ShowPlant();
        customer.ShowCharacter();
        CheckIfSame(customerList);
        levelMang.numCustomers--;
      //  AudioManager.Instance.GetFreeSource().PlayOneShot(happyCustomerSound);
    }

    /// <summary>
    /// Checks if the two active customers are the same.
    /// ONLY WORKS WHEN <c>customerList</c> HAS TWO CUSTOMERS
    /// </summary>
    /// <param name="listOfCust">The list of active customers.</param>
    public void CheckIfSame(Customer[] listOfCust)
    {
        if (listOfCust.Length >= 2){
            Debug.Log("Customer 1: " + listOfCust[0].charWanted);
            Debug.Log("Customer 2: " + listOfCust[1].charWanted);
            sameCustomer = listOfCust[0].charWanted.Equals(listOfCust[1].charWanted);
            Debug.Log("Check if same: " + sameCustomer);
        }
    }

    /// <summary>
    /// Uses <see cref="CurrentState"/> to determine which
    /// animations the customers will use.
    /// </summary>
    public void CheckAnimationState()
    {
        SetCustomerState();

        switch (CurrentState)
        {
            case CustomerState.Nervous:
                Debug.Log("ZOMG IM SO NERVOUS DAD");
                break;
            case CustomerState.Normal:
                Debug.Log("Hi, how are ya?");
                break;
            default:
                Debug.Log("00111100 00001111 11100001");
                break;
        }
    }

    /// <summary>
    /// Uses <see cref="IsNervous"/> to determine what the value
    /// of <see cref="CurrentState"/> is going to be.
    /// </summary>
    public void SetCustomerState()
    {
        CurrentState = IsNervous() ? CustomerState.Nervous : CustomerState.Normal;
    }
    #endregion

    #region ScoreManager Getters
    /// <summary>
    /// Gets the value of <see cref="ScoreManager.RemainingMoves"/>
    /// and checks if the value is less than or equal to 5.
    /// </summary>
    /// <returns> Whether or not there are 5 or less moves remaining.</returns>
    public bool IsNervous()
    {

        return levelMang.moveLimit <= 5;
    }
    #endregion
}
