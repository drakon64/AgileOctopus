using System.Net.Http.Json;
using AgileOctopus;

var httpClient = new HttpClient();

var standardUnitRates = await httpClient.GetFromJsonAsync<StandardUnitRates>(
    "https://api.octopus.energy/v1/products/AGILE-FLEX-22-11-25/electricity-tariffs/E-1R-AGILE-FLEX-22-11-25-C/standard-unit-rates/?period_from=2023-03-26T00:00Z&period_to=2023-03-26T01:29Z",
    SourceGenerationContext.Default.StandardUnitRates
);

foreach (var rate in standardUnitRates!.Results)
{
    Console.WriteLine(rate);
}
