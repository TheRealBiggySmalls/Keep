﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scenario : MonoBehaviour {

	public void OptionOne(){
		Close();
		ScenarioManager.DisplayResults(1);
	}

	public void OptionTwo(){
		Close();
		ScenarioManager.DisplayResults(2);
	}

	public void Close(){
		Destroy(gameObject);
	}

	//list option outcomes in here
}
