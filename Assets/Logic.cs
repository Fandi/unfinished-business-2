using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using UnityEngine;

public class Logic : MonoBehaviour
{
    XPathNavigator _navigator;

    public TextAsset xslFile;
    [Range(1f, 4f)]
    public int selectedItem;

    void Awake()
    {
        string dataXml = "<items><item id='1'><name><![CDATA[Item 1]]></name><price>100</price></item><item id='2'><name><![CDATA[Item 2]]></name><price>120</price></item><item id='3'><name><![CDATA[Item 3]]></name><price>150</price></item><item id='4'><name><![CDATA[Item 4]]></name><price /></item></items>";

        using (StringReader reader = new StringReader(dataXml))
        {
            _navigator = new XPathDocument(reader).CreateNavigator();
        }
    }

    void Reset()
    {
        xslFile = null;
        selectedItem = 1;
    }

    void Start()
    {
        XslCompiledTransform xslCompiledTransform = new XslCompiledTransform(/*true*/);

        using (StringReader reader = new StringReader(xslFile.text))
        {
            using (XmlReader stylesheet = XmlReader.Create(reader))
            {
                xslCompiledTransform.Load(stylesheet);
            }
        }

        XsltArgumentList args = new XsltArgumentList();
        args.AddExtensionObject("urn:module", this);

        using (StringReader reader = new StringReader("<data />"))
        {
            using (XmlReader input = XmlReader.Create(reader))
            {
                using (FauxWriter output = new FauxWriter())
                {
                    xslCompiledTransform.Transform(input, args, output);
                }
            }
        }
    }

    public XPathNodeIterator GetAllItems()
    {
        return _navigator.Select("/items/item");
    }

    public XPathNodeIterator GetItem(int id)
    {
        string xpath = string.Format("/items/item[@id = '{0}']", id);
        return _navigator.Select(xpath);
    }

    public int GetSelectedItemId()
    {
        return selectedItem;
    }

    public bool Log(string message)
    {
        Debug.Log(message);
        return true;
    }

    public IEnumerator StartCoroutine()
    {
        yield break;
    }
}
