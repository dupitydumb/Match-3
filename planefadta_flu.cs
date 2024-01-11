using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class planefadta_flu : MonoBehaviour
{
	public void Scene_Laod_eef() => SceneManager.LoadScene(8);
	public void Scene_Laod_eef1() => SceneManager.LoadScene(9);
	public void Scene_Laod_eef2() => SceneManager.LoadScene(12); 
	public void Scene_Laod_loader() => SceneManager.LoadScene(1); 
	public void Scene_Laod_eef2(int level_name) => SceneManager.LoadScene(level_name); 
	public void Open_Privacy_Link() => Application.OpenURL("https://pages.flycricket.io/flight-simulator-202/privacy.html");   
}
