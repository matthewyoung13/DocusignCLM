string docsXml   = _context.XmlVariables["FolderDocuments"].ValueString;
string typesRaw  = _context.XmlVariables["PackageTypes"].ValueString;
string isdaId    = _context.XmlVariables["ISDA_DocID"].ValueString;
string nettingId = _context.XmlVariables["NettingReview_DocID"].ValueString;

System.Collections.Generic.List<string> ids = new System.Collections.Generic.List<string>();
if (!string.IsNullOrEmpty(isdaId)) ids.Add(isdaId.Trim());
if (!string.IsNullOrEmpty(nettingId) && !ids.Contains(nettingId.Trim())) ids.Add(nettingId.Trim());

if (string.IsNullOrEmpty(docsXml) || string.IsNullOrEmpty(typesRaw))
{
    return string.Join("|", ids);
}

string[] keep = typesRaw.Split('|');

System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
doc.LoadXml(docsXml);

foreach (System.Xml.XmlNode d in doc.SelectNodes("//Document"))
{
    System.Xml.XmlNode idNode = d.SelectSingleNode("Id");
    if (idNode == null) continue;

    string value = null;
    foreach (System.Xml.XmlNode n in d.SelectNodes(".//*"))
    {
        string name   = n.LocalName.Replace("_", "").ToLower();
        string parent = n.ParentNode == null ? "" : n.ParentNode.LocalName.Replace("_", "").ToLower();
        if (name == "typeofdocument" && parent == "isdadocuments")
        {
            value = n.InnerText.Trim();
            break;
        }
    }
    if (value == null) continue;

    foreach (string k in keep)
    {
        if (string.Equals(value, k.Trim(), System.StringComparison.OrdinalIgnoreCase))
        {
            string id = idNode.InnerText.Trim();
            if (id != "" && !ids.Contains(id)) ids.Add(id);
            break;
        }
    }
}

return string.Join("|", ids);
