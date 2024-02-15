using System;

public struct RankingData 
{
    public string Name {  get; set; }
    public int Score {  get; set; }
    public int Difficulty { get; set; }//0 Easy 1 Normal 2 Hard

    public RankingData(string name, int score, int difficulty) 
    { 
        Name= name;
        Score= score;
        Difficulty = difficulty;
    }

    public bool Equals(RankingData currentData)
    {
        return Equals(currentData.Name, currentData.Score, currentData.Difficulty);
    }

    public bool Equals(string name, int score, int difficulty)
    {
        return name.Equals(Name, StringComparison.InvariantCulture) && score == Score && difficulty == Difficulty;
    }
}
