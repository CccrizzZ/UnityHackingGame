using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ColumnScript : MonoBehaviour
{
    // array of all text
    public List<Text> TextArray;
    
    // previous random selection
    Text PreviousSelectedText;

    // column hacked
    public bool Hacked = false;

    // key of this column 
    public string keychar;

    // alphabet
    string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public float RollingInterval;

    void Start()
    {
        InitTextArray();
    }


    void Update()
    {
    }



    void InitTextArray()
    {

        // get all child char and push them into child array
        for (var i = 0; i < transform.childCount; i++)
        {
            var temp = transform.GetChild(i).GetComponent<Text>();
            TextArray.Add(temp);


        }

        // start the rolling
        InvokeRepeating(nameof(RandomizeArray), 0.0f, RollingInterval);


    }


    // will becalled every 1 second
    void RandomizeArray()
    {
        
        // get random item in child text
        var rand = TextArray[Random.Range(0, TextArray.Count)];
        
        // if same text selected, return
        if (PreviousSelectedText == rand)return;

        // set random selection color to red and text to key
        rand.color = Color.red;
        rand.text = keychar;
        // print(rand.text);

        // set all other letter to random
        foreach (var item in TextArray)
        {
            if (item != rand)
            {
                item.text = GetRandomLetter();
                item.color = Color.green;
            }
        }

        



        // set previous random result
        PreviousSelectedText = rand;

    }



    // get random letter from alphabet
    string GetRandomLetter()
    {
        return Alphabet[Random.Range(0, Alphabet.Length)].ToString();
    }

    // if cursor on this column
    public void setColumnActiveColor()
    {
        GetComponent<Image>().color = new Color
        (
            GetComponent<Image>().color.r,
            GetComponent<Image>().color.g,
            GetComponent<Image>().color.b,
            1.0f
        );
    }

    // if cursor not on this column
    public void setColumnDisableColor()
    {
        GetComponent<Image>().color = new Color
        (
            GetComponent<Image>().color.r,
            GetComponent<Image>().color.g,
            GetComponent<Image>().color.b,
            0.5f
        );
    }


    // stop rolling if hacked
    public void SetHacked()
    {
        Hacked = true;
        CancelInvoke(nameof(RandomizeArray));

    }

    // start rolling if unhacked
    public void SetUnHacked()
    {
        Hacked = false;
        InvokeRepeating(nameof(RandomizeArray), 0.0f, 1.0f);
    }
}
