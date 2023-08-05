using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Achievement 
{
    public string Id;
    public string Title;
    public string Description;

    public Achievement(string Id,string Title, string Description)
    {
        this.Id = Id;
        this.Title = Title;
        this.Description = Description;
    }
}
