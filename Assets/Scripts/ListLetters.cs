using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListLetters
{
    private string name;
    private float TimeAverage;
    private float NewTime;
    public int Correct;
    public int Incorrect;
    private float accuracy;
    public float Count;
    private float Speed;

    public float AverageAccuracy = 0;
    public float AverageSpeed = 0;
    public float Loop = 0;

    public ListLetters(string name, float TimeAverage, float NewTime, float Count)
    {
        this.name = name;
        this.TimeAverage = TimeAverage;
        this.NewTime = NewTime;
        this.Correct = 0;
        this.Incorrect = 0;
        this.Count = Count;
    }

    public string GetAllData
    {
        get { return this.name + " Correct: " + this.Correct + " Incorrect: " + this.Incorrect + " Accuracy: " + this.accuracy + "%" + " \n Speed: " + this.Speed + " secs"; }
        //get { return this.name + " Correct: " + this.Correct + " Incorrect: " + this.Incorrect + " Accuracy: " + (this.Correct/ this.Count)*100 + "%"; }

    }

    public string getName
    {
        get { return name; }
    }

    public float getCount
    {
        get { return Count; }
    }

    public int GetWrongData
    {
        get { return Incorrect; }
    }

    public int GetCorrect
    {
        get { return Correct; }
    }
    public float GetAccuracy
    {
        get { return accuracy; }
    }
    public float GetSpeed
    {

        get 
        { 
            if(Count == 0)
                return 0;
            return (Speed/Count); 
        }
    }
    public void UpdateWrongLetterData()
    {
        this.Incorrect += 1;
        this.Count += 1;

    }

    public void UpdateData()
    {
        this.Correct += 1;
        this.Count += 1;
    }
    public void UpdateAccuracy()
    {
        if(this.Count==0)
            this.accuracy = 0;
        this.accuracy = (this.Correct / this.Count) * 100;
    }
    public void UpdateSpeed(float speedInput)
    {
        this.Speed += speedInput ;
        //this.Speed = this.Speed / this.Count;
        //float _2Diggit = (float)Math.Round((Math.Truncate((double)Speed * 100.0) / 100.0));
        //this.Speed = _2Diggit;
        Debug.Log("Speed_ : " + Speed);

    }
    public void GetAverageAccuracyAndSpeed()
    {
        this.AverageAccuracy = (this.Correct / this.Count)*100;
        this.AverageSpeed = this.AverageSpeed / this.Loop;

        float _2DiggitAccuracy = (float)(Math.Truncate((double)AverageAccuracy * 100.0) / 100.0);
        float _2DiggitSpeed = (float)(Math.Truncate((double)AverageSpeed * 100.0) / 100.0);

        this.AverageAccuracy = _2DiggitAccuracy;
        this.AverageSpeed = _2DiggitSpeed;

    }

}
