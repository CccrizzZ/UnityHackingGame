using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class HackingArea : MonoBehaviour
{
    // key phrases
    string[] phrases = {"FUBAR", "HCKDD", "BRUTE", "GOTYA", "BREAK"};

    // sound manager ref
    SoundManager SManager;

    // pass word characters colums ref and count
    public GameObject columnRef;
    int EasyColCount = 5;
    
    // list of all column
    public List<GameObject> ColumnArray;

    // cursor position for column array
    int CursorPosition = 0;

    // chances
    int EasyChances = 3;
    int Chances;
    Text ChancesText;
    
    // timer
    TimerScript TScript;
    

    // result panel
    public GameObject HackedPanel;
    public GameObject FailedPanel;




    void Start()
    {
        // get timer
        TScript = transform.parent.transform.parent.GetComponent<TimerScript>();
        
        // hide the panels
        HackedPanel.SetActive(false);
        FailedPanel.SetActive(false);


        // get sound manager
        SManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();

        // set total chances
        Chances = EasyChances;

        // set ui ref
        ChancesText = GameObject.FindGameObjectWithTag("ChancesText").GetComponent<Text>();
        ChancesText.text = Chances.ToString();


        // get random phrase
        var targetphrase = phrases[Random.Range(0, phrases.Length)];


        // add columns to the hacking area
        for (var i = 0; i < EasyColCount; i++)
        {
            // create column
            var temp = Instantiate(columnRef, transform);

            // set the key word to random phrase in phrases
            temp.GetComponent<ColumnScript>().keychar = targetphrase[i].ToString();
            
            // add it to the column array
            ColumnArray.Add(temp);
        }

        // print(phrases[Random.Range(0, phrases.Length)]);

        // set cursor position to 0
        SetActiveColumnUnderCursor(0);
    }

    void Update()
    {
        
        // // left arrow
        // if (Input.GetKeyDown(KeyCode.LeftArrow))
        // {


        // }

        // // right arrow
        // if (Input.GetKeyDown(KeyCode.RightArrow))
        // {


        // }


        // enter key
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // print("Unlock");
            TryPick();
        }

    }


    void SetActiveColumnUnderCursor(int direction)
    {
        // set previous column at the opposite of travel direction to disable
        ColumnArray[CursorPosition - direction].GetComponent<ColumnScript>().setColumnDisableColor();
        // active current column
        ColumnArray[CursorPosition].GetComponent<ColumnScript>().setColumnActiveColor();
    }


    // cursor go next column
    void NextColumn()
    {
        if (CursorPosition + 1 < ColumnArray.Count)
        {
            CursorPosition++;
            // print(CursorPosition);
            SetActiveColumnUnderCursor(1);
        }
    }


    // cursor go previous column
    void PrevColumn()
    {
        if (CursorPosition - 1 >= 0)
        {
            CursorPosition--;
            // print(CursorPosition);

            ColumnArray[CursorPosition].GetComponent<ColumnScript>().SetUnHacked();
            SetActiveColumnUnderCursor(-1);
        }
    }


    // enter key event
    void TryPick()
    {
        // if failed dont move
        if (Chances <= 0)return;

        // get cursored column script
        var temp = ColumnArray[CursorPosition].GetComponent<ColumnScript>();
        var condition = temp.TextArray[1].text == temp.keychar && temp.TextArray[1].color == Color.red;
        



        // if pick success, goto next column
        if (condition)
        {
            temp.SetHacked();

            if (CursorPosition >= EasyColCount - 1)
            {
                // Hacking succeed
                print("succeed");

                // play sound effect
                SManager.HackingSucceed.Play();
                StopTimer();

                // show close popup
                HackedPanel.SetActive(true);
                GameObject.FindGameObjectWithTag("TimeUsedText").GetComponent<Text>().text = TScript.GetTimeUsed().ToString();
            
            }
            else
            {
                // goto next column and play sound effect
                NextColumn();
                SManager.KeyCorrect.Play();
            }

        }
        else
        {
            
            // decrement chances and update ui
            Chances--;
            ChancesText.text = Chances.ToString();

            // failed to hack if no chance
            if (Chances <= 0)
            {
                HackingFailed();
            }
            else
            {
                // if failed goback to the previous column
                if (CursorPosition > 0)
                {
                    PrevColumn();
                }

                // play sound effect
                SManager.KeyError.Play();

            }
        }
    }



    public void HackingFailed()
    {
        // Hacking failed
        print("Failed");
        
        // show popup and play sound effect
        SManager.HackingFailed.Play();
        
        // stop timer
        StopTimer();

        // show the panel
        FailedPanel.SetActive(true);

    }




    void StopTimer()
    {
        TScript.Ticking = false;
    }



}
