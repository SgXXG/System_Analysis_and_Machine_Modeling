namespace saimon3;

public static class Extensions
{
    public static List<int> ToSumList(this List<List<int>> list)
    {
        var sumList = new List<int>();

        if (list.Count <= 0) return sumList;
        
        for (var i = 0; i < list[0].Count; i++)
        {
            sumList.Add(list.Sum(l => l[i]));
        }

        return sumList;
    }
}