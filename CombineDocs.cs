string existing = _context.XmlVariables["CombinedDocIDs"].ValueString;

System.Xml.XmlDocument doc2 = new System.Xml.XmlDocument();
doc2.LoadXml(_context.XmlVariables["AmendmentsDocuments"].ValueString);

System.Collections.Generic.List<string> ids = new System.Collections.Generic.List<string>();

if (!string.IsNullOrEmpty(existing))
{
    foreach (string s in existing.Split('|'))
    {
        if (s.Trim() != "") ids.Add(s.Trim());
    }
}

foreach (System.Xml.XmlNode n in doc2.SelectNodes("//Document/Id"))
{
    string id = n.InnerText.Trim();
    if (id != "" && !ids.Contains(id)) ids.Add(id);
}

return string.Join("|", ids);
