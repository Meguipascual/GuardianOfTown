using System;

public struct RankingData 
{
    public string Name {  get; set; }
    public int Score {  get; set; }

    public RankingData(string name, int score) 
    { 
        Name= name;
        Score= score;
    }

    public bool Equals(RankingData currentData)
    {
        return Equals(currentData.Name, currentData.Score);
    }

    public bool Equals(string name, int score)
    {
        return name.Equals(Name, StringComparison.InvariantCulture) && score == Score;
    }
}
