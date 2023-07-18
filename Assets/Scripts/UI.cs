using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
   
}
