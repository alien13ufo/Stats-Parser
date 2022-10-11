using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public Menu CurrentMenu;


	// Use this for initialization
	void Start () {
		showMenu(CurrentMenu);
	}

	public void showMenu(Menu menu)
	{
		if(CurrentMenu != null)
			CurrentMenu.IsOpen = false;

		CurrentMenu = menu;
		CurrentMenu.IsOpen = true;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
