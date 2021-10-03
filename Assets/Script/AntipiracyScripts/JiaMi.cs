using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI; 

public class JiaMi : MonoBehaviour {
	public static string JiaMijson;
	public TestJSON testJSON;
	public string json;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnClick(){
		InputField input1=GameObject.Find("InputField1").GetComponent<InputField>(); //将文本输入框内容存入 license
		Http.license=input1.textComponent.text ;
		//如果对话框内容为空，提示
		if(input1.textComponent.text ==""){
			Text txt1 = GameObject.Find("Outcome").GetComponent<Text> ();
			txt1.text ="请输入序列号";
		}

			testJSON = new TestJSON();
			json = JsonUtility.ToJson(testJSON);
			JiaMijson=Base64Encode(json);
			JiaMijson = "bnuarlab" + JiaMijson;
			JiaMijson=ReverseA(JiaMijson);
//		Debug.Log (JiaMijson);
	}
		

	//Base64解密算法
	public static string Base64Decode(string str)  
	{  
		byte[] bytes = Convert.FromBase64String(str);  
		bytes = Convert.FromBase64String(Encoding.Default.GetString(bytes));  
		return Encoding.Default.GetString(bytes);  
	}  

	//Base64加密算法
	public static string Base64Encode(string str)  
	{  
		string go = Convert.ToBase64String(Encoding.Default.GetBytes(str));  
		return go;  
	}  

	//将字符串倒序的算法
	public string ReverseA(string text)
	{
		char[] cArray = text.ToCharArray();
		string reverse = String.Empty;
		for (int i = cArray.Length - 1; i > -1; i--)
		{
			reverse += cArray[i];
		}
		return reverse;
	}

}
