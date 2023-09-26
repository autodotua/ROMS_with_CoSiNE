string template = @"  - variable:       {0}                                             # {1}
    standard_name:  {2}
    long_name:      {2}
    units:          {3}                                            # [{4}]
    field:          {5}
    time:           {6}
    index_code:     {7}
    type:           {8}
    add_offset:     0.0d0
    scale:          {9}";

 List<VarInfo> vars = new List<VarInfo>();
int varPropIndex = -1;
foreach (var line in File.ReadLines("varinfo.dat"))
{
    if (line.StartsWith('\''))
    {
        VarInfo varInfo = new VarInfo();
        vars.Add(varInfo);
        varPropIndex = 0;
        varInfo.varName = line[(line.IndexOf('\'') + 1)..line.IndexOf('\'',1)];
        varInfo.IO = line[(line.IndexOf("!") + 1)..].Trim();
        Console.WriteLine(varInfo.varName);
    }
    else if(varPropIndex>=0) 
    {
        varPropIndex++;
        switch (varPropIndex)
        {
            case 1:
                vars[^1].longName= line[(line.IndexOf('\'') + 1)..line.IndexOf('\'', 3)];
                break;
            case 2:
                vars[^1].units = line[(line.IndexOf('\'') + 1)..line.IndexOf('\'', 3)];
                vars[^1].shortUnits = line[(line.IndexOf('[')+1)..line.IndexOf(']', 1)];
                break;
            case 3:
                vars[^1].field = line[(line.IndexOf('\'')+1)..line.IndexOf(',', 3)];
                break;
            case 4:
                vars[^1].time = line[(line.IndexOf('\'') + 1)..line.IndexOf('\'', 3)];
                break;
            case 5:
                vars[^1].index = line[(line.IndexOf('\'') + 1)..line.IndexOf('\'', 3)];
                break;
            case 6:
                vars[^1].type = line[(line.IndexOf('\'') + 1)..line.IndexOf('\'', 3)];
                break;
            case 7:
                vars[^1].scale = line.Trim();
                varPropIndex = -1;
                break;
        }

    }
}

using var file = File.OpenWrite("varinfo.yaml");
using var text = new StreamWriter(file);
foreach (var v in vars)
{
    text.WriteLine(string.Format(template, v.varName, v.IO, v.longName, 
        v.units, v.shortUnits, v.field, v.time, v.index, v.type, v.scale));
    text.WriteLine();
}
text.Flush();
text.Close();

public class VarInfo
{
    public string varName;
    public string longName;
    public string units;
    public string field;
    public string time;
    public string index;
    public string type;
    public string scale;
    public string IO;
    public string shortUnits;
}