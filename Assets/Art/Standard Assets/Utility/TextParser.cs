using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TextParser {


	public static List<pointOfSail> Parse(TextAsset csvString){
		//get the array of lines
		List<pointOfSail> arrayPointOfSail = new List<pointOfSail>();
		string[] arrayOfLines = csvString.ToString().Split ("\n" [0]);

		//take each line, split by comma, and then populate list at that index
		for (int i=0; i<arrayOfLines.Length; i++) {
			string[] arrayOfStrings = arrayOfLines[i].Split(","[0]);
			pointOfSail tempPOS =  new pointOfSail(arrayOfStrings[0],float.Parse(arrayOfStrings[1]));
			arrayPointOfSail.Add(tempPOS);
		}
		return arrayPointOfSail;
	}
}

public class Conversation () {

}