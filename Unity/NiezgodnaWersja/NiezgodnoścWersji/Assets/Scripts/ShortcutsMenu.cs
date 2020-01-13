using UnityEngine;
using System.Collections;

public class ShortcutsMenu : MonoBehaviour
{
void Start(){

}

    void OnGUI()
    {
		GUIStyle customButton = ("button");
customButton.fontSize = 18;
        // Make a button using a custom GUIContent parameter to pass in the tooltip.
        GUI.Button(new Rect(0, 718, 150, 50), new GUIContent("Shortcuts", "Press C to switch camera \nPress R to restart the scene"));
	
GUIStyle customLabel = ("label");
customLabel.fontSize = 12;
customLabel.normal.textColor = Color.green ;
        GUI.Label(new Rect(0, 678, 150, 70), GUI.tooltip);
    }
}