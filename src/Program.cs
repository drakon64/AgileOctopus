using System.Net.Http.Json;
using AgileOctopus;

string validFrom;
string validTo;

{
    var now = DateTime.Now.AddDays(1);

    validFrom = now.Date.ToUniversalTime().ToString("O");

    validTo = now
        .Date.Add(new TimeSpan(hours: 23, minutes: 59, seconds: 0))
        .ToUniversalTime()
        .ToString("O");
}

StandardUnitRates standardUnitRates;

{
    var httpClient = new HttpClient();

    standardUnitRates = (
        await httpClient.GetFromJsonAsync<StandardUnitRates>(
            $"https://api.octopus.energy/v1/products/AGILE-24-10-01/electricity-tariffs/E-1R-AGILE-24-10-01-A/standard-unit-rates/?period_from={validFrom}&period_to={validTo}",
            SourceGenerationContext.Default.StandardUnitRates
        )
    )!;

    httpClient.Dispose();
}

foreach (var rate in standardUnitRates.Results.Reverse())
{
    Console.WriteLine(rate);
}
