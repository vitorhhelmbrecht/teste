using Newtonsoft.Json;

public class Program
{
    private static readonly string FootbalMatchesApiUrl = "https://jsonmock.hackerrank.com/api/football_matches";

    public async static Task Main() {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await GetTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await GetTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public async static Task<int> GetTotalScoredGoals(string team, int year) {
        var scoredGoalsFromSide1 = await GetGoalsAsOneOfTheSides("team1", team, year);
        var scoredGoalsFromSide2 = await GetGoalsAsOneOfTheSides("team2", team, year);

        return scoredGoalsFromSide1 + scoredGoalsFromSide2;
    }

    public async static Task<int> GetGoalsAsOneOfTheSides(string teamSide, string team, int year) {
        HttpClient client = new();
        int currentPage = 0;
        int totalGoals = 0;
        int totalPages;

        do {
            // I considered trying to make this do while into a recursive method, but after some simple performace testing with
            // StopWatch, the do while took basically half of the time the recursive method took to get and process the data.

            currentPage++;

            var getResult = await client.GetAsync($"{FootbalMatchesApiUrl}?{teamSide}={team}&year={year}&page={currentPage}");
            var apiResponseSerialized = await getResult.Content.ReadAsStringAsync();

            if (apiResponseSerialized == null) {
                Console.WriteLine($"An error occured while trying to search for the scores of the team{team}, side {teamSide}, year {year} and page number {currentPage}.\r\n" +
                    $"Api response: {apiResponseSerialized}");
                return 0;
            }

            ApiResponseDto apiResponse = JsonConvert.DeserializeObject<ApiResponseDto>(apiResponseSerialized);

            if (apiResponse == null) {
                Console.WriteLine($"An error occured while desserializing the API response of the team {team}, side {teamSide}, year {year} and page number {currentPage}." +
                    $"\r\nResposta: {apiResponseSerialized}");
                return 0;
            }

            foreach (GameDto game in apiResponse!.Games) {
                if (teamSide == "team1")
                    totalGoals += game.Team1Goals;
                else
                    totalGoals += game.Team2Goals;
            }

            totalPages = apiResponse.TotalPages;
        } while (currentPage < totalPages);

        return totalGoals;
    }

    public class ApiResponseDto
    {
        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }
        [JsonProperty("data")]
        public List<GameDto> Games { get; set; }
    }

    public class GameDto
    {
        [JsonProperty("team1goals")]
        public int Team1Goals { get; set; }
        [JsonProperty("team2goals")]
        public int Team2Goals { get; set; }
    }
}