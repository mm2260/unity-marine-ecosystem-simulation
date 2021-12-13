using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreatureManager : MonoBehaviour {

	public List<CoreSystem> list;

    public List<Plant> PlantList;

    public TMP_Text day;

    private void Update()
    {
         day.text = "Day: " + ((int) (Time.time / 10f)) ;
    }


}
