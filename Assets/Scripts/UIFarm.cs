﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFarm : GameController 
{
	public bool playerUI;
    public bool cowUI;
	public bool loadSaveUI;

	public GUIStyle buttonBuy;
	public GUIStyle buttonSell;
	public GUIStyle buttonInfo;
	public GUIStyle buttonLoad;
	public GUIStyle buttonSave;
	public GUIStyle buttonCattle;
	public GUIStyle buttonExit;
	public GUIStyle buttonFeed;
	public GUIStyle labelAge;
	public GUIStyle labelBreed;
	public GUIStyle labelHappiness;
	public GUIStyle labelHealth;
	public GUIStyle labelPregnant;
	public GUIStyle labelGender;
	public GUIStyle labelWeight;
	public GUIStyle customTextStyle;

    public Cow cow;
	public GameObject cowGameObject;
	string cowGender = "Male";
	
	public Image healthBar;
	public Image happinessBar;

	public Color backgroundColor = new Color(0.2f, 0.3f, 0.4f, 0.5f);
	public Color foregroundColor = new Color(0.2f, 0.3f, 0.4f, 0.5f);

	public Image[] bars = new Image[2];

	public GameObject AudioObject;

	Rect windowRect;

    void Start()
    {
        playerUI = true;
		bars = GetComponentsInChildren<Image>();
		healthBar = bars[0];
		happinessBar = bars[1];

		AudioObject = GameObject.Find("Background Music");
		if(AudioObject != null)
			Destroy(AudioObject);
    }

    void OnGUI()
    {
		GUI.color = foregroundColor;

		if(playerUI == false && cowUI == false && loadSaveUI == false)
		{
			windowRect = new Rect(Screen.width * .4f, Screen.height - 400f, 355, 345);
			windowRect = GUI.Window(0, windowRect, CowMoreInfo, "Cow Info");
		}
		else
		{
			windowRect = new Rect(Screen.width * .29f, Screen.height - 150f, 620, 100);
		}
		
        if (playerUI)
        {
			windowRect = GUI.Window(0, windowRect, PlayerInfo, "Farm");
		}
		else if (cowUI && cowGameObject != null )
        {
			windowRect = GUI.Window(0, windowRect, CowInfo, cow.name);
		}
		else if (loadSaveUI)
		{
			windowRect = GUI.Window(0, windowRect, LoadSaveInfo, "Load / Save");
		}
	}

    void PlayerInfo(int windowID)
    {
		GUI.contentColor = backgroundColor;

		GUI.Label(new Rect(20, 25, 150, 30), game.player.name, customTextStyle);
        GUI.Label(new Rect(20, 60, 150, 30), "Cash: € " + game.player.cash, customTextStyle);

		SetHealth(0);
		SetHappiness(0);
		
		if (GUI.Button(new Rect(270, 30, 150, 50), "", buttonCattle))
		{
			Vector3 spawnLocation = new Vector3(Random.Range(55f, 105f), 0, Random.Range(220f, 265f));
			cow = CowMaker.GenerateCow(spawnLocation);
		}
    }

    void CowInfo(int windowID)
    {
		GUI.contentColor = backgroundColor;

        GUI.Label(new Rect(165, 25, 150, 30), "Health:", customTextStyle);
		GUI.Label(new Rect(165, 60, 150, 30), "Happiness:", customTextStyle);

		SetHealth(cow.health / 100f);
		SetHappiness(cow.happiness / 10f);

		healthBar.transform.position = new Vector2 (windowRect.center.x + 100, 110);
		happinessBar.transform.position = new Vector2 (windowRect.center.x + 100, 75);

        /*if (!cow.ownedByPlayer)
			if (GUI.Button(new Rect(50, 30, 80, 50), "", buttonBuy))
				cow.Buy();
		
        if (cow.ownedByPlayer)
		{
            if (GUI.Button(new Rect(50, 30, 80, 50), "", buttonSell))
            {
                cow.Sell();
                Destroy(cowGameObject);
                cow.gameObjectID = 0;
                playerUI = true;
                cowUI = false;
            }
		}*/

		if (GUI.Button(new Rect(40, 30, 90, 50), "", buttonInfo))
		{
			cowUI = false;

		}

		if (GUI.Button(new Rect(535, 30, 50, 50), "", buttonExit))
		{
			CameraController.ResetCamera(GameObject.Find("Main Camera").transform.position);
			playerUI = true;
			cowUI = false;
			Movement.freeRoam = true;
		}
    }

	void CowMoreInfo(int windowID)
	{
		GUI.contentColor = backgroundColor;

		SetHealth(cow.health / 100f);
		SetHappiness(cow.happiness / 10f);

		healthBar.transform.position = new Vector2 (windowRect.center.x + 70, 280);
		happinessBar.transform.position = new Vector2 (windowRect.center.x + 70, 245);
		
		if (!cow.gender == true)
		{
			cowGender = "Female";
		}
		else
		{
			cowGender = "Male";
			cow.pregnant = false;
		}

		GUI.Label(new Rect(35, 40, 90, 25), "", labelAge);
		GUI.Label(new Rect(142, 40, 150, 30), "" + cow.age, customTextStyle);
		GUI.Label(new Rect(35, 72, 90, 25), "", labelBreed);
		GUI.Label(new Rect(142, 72, 150, 30), "" + cowGender, customTextStyle);
		GUI.Label(new Rect(35, 105, 110, 30), "", labelHappiness);
		GUI.Label(new Rect(35, 140, 90, 25), "", labelHealth);
		GUI.Label(new Rect(35, 175, 110, 30), "", labelPregnant);
		GUI.Label(new Rect(160, 175, 150, 30), "" + cow.pregnant, customTextStyle);
		GUI.Label(new Rect(35, 205, 90, 25), "", labelGender);
		GUI.Label(new Rect(142, 205, 150, 30), "" + cow.gender, customTextStyle);
		GUI.Label(new Rect(35, 240, 90, 30), "", labelWeight);
		GUI.Label(new Rect(142, 240, 150, 30), "" + cow.weight, customTextStyle);

		if (GUI.Button(new Rect(38, 288, 80, 40), "", buttonFeed))
		{
			int feedAmount = Random.Range(1, 6);
			
			switch(feedAmount)
			{
			case 1:
				cow.happiness = cow.happiness + 1;
				cow.health = cow.health + 5;
				break;
			case 2:
				cow.happiness = cow.happiness + 2;
				cow.health = cow.health + 10;
				break;
			case 3:
				cow.happiness = cow.happiness + 3;
				cow.health = cow.health + 15;
				break;
			case 4:
				cow.happiness = cow.happiness + 4;
				cow.health = cow.health + 20;
				break;
			case 5:
				cow.happiness = cow.happiness + 5;
				cow.health = cow.health + 25;
				break;
			case 6:
				cow.happiness = cow.happiness + 6;
				cow.health = cow.health + 30;
				break;
			}

			if(cow.happiness > 10)
				cow.happiness = 10;

			if(cow.health > 100)
				cow.health = 100;
		}

		if (GUI.Button(new Rect(285, 280, 50, 50), "", buttonExit))
			cowUI = true;
	}

	void LoadSaveInfo(int windowID)
	{
		SetHealth(0);
		SetHappiness(0);

		GUI.contentColor = backgroundColor;
		
		if (GUI.Button(new Rect(50, 30, 150, 50), "", buttonLoad))
			game.Load();
		
		if (GUI.Button(new Rect(275, 30, 150, 50), "", buttonSave))
			game.Save();
	}

	void SetHealth(float health)
	{
		healthBar.fillAmount = health;
	}

	void SetHappiness(float happiness)
	{
		happinessBar.fillAmount = happiness;
	}
}