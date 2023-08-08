using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Achievement 
{
    public string Id;
    public string Title;
    public string Description;
    public string State;

    public Achievement(string Id,string Title, string Description, string State)
    {
        this.Id = Id;
        this.Title = Title;
        this.Description = Description;
        this.State = State;
    }
}
