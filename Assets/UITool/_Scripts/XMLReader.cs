using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Text;

public class XMLReader : MonoBehaviour
{
    public TextAsset dictionary;
    public string languageName;
    public static int currentLanguage = 0;
    
     public static List<Dictionary<string, string>> languages = new List<Dictionary<string, string>>();
     Dictionary<string, string> obj;


     private void Awake()
     {
         Reader();
     }

     private void Update()
     {
         languages[currentLanguage].TryGetValue("name", out languageName);
         
     }

     void Reader()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(dictionary.text);
        XmlNodeList languagesList = xmlDoc.GetElementsByTagName("language");
        foreach (XmlNode languageValue in languagesList)
        {
            XmlNodeList languageContent = languageValue.ChildNodes;
            obj = new Dictionary<string, string>();
            foreach (XmlNode value in languageContent)
            {
                if (value.Name == "name")
                {
                    obj.Add("name", value.InnerText);
                }
                if (value.Name == "jouer")
                {
                    obj.Add("jouer", value.InnerText);
                }
                if(value.Name == "options")
                {
                    obj.Add("options", value.InnerText);
                }
                if(value.Name == "quitter")
                {
                    obj.Add("quitter", value.InnerText);
                }
                if(value.Name == "retour")
                {
                    obj.Add("retour", value.InnerText);
                }
                if(value.Name == "langue")
                {
                    obj.Add("langue", value.InnerText);
                }
                if(value.Name == "son")
                {
                    obj.Add("son", value.InnerText);
                }
                if(value.Name == "musique")
                {
                    obj.Add("musique", value.InnerText);
                }
                if(value.Name == "ecran")
                {
                    obj.Add("ecran", value.InnerText);
                }
                if(value.Name == "pleinEcran")
                {
                    obj.Add("pleinEcran", value.InnerText);
                }
                if(value.Name == "fenetré")
                {
                    obj.Add("fenetré", value.InnerText);
                }
                if(value.Name == "sansBordure")
                {
                    obj.Add("sansBordure", value.InnerText);
                }
                if(value.Name == "menuPrincipal")
                {
                    obj.Add("menuPrincipal", value.InnerText);
                }
                if(value.Name == "reprendre")
                {
                    obj.Add("reprendre", value.InnerText);
                }
                if (value.Name == "interaction")
                {
                    obj.Add("interaction", value.InnerText);
                }
                if(value.Name == "coffreDesc")
                {
                    obj.Add("coffreDesc", value.InnerText);
                }
                if(value.Name == "panneauDesc")
                {
                    obj.Add("panneauDesc", value.InnerText);
                }
            }
            languages.Add(obj);
        }
    }
}
