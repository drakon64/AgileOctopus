using System.Net.Http.Json;
using AgileOctopus.Octopus;

string validFrom;
string validTo;

{
    var now = DateTime.Now.AddDays(1).Date;

    validFrom = now.ToUniversalTime().ToString("O");

    validTo = now.Add(new TimeSpan(hours: 23, minutes: 59, seconds: 0))
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

var rates = standardUnitRates.Results.Reverse().ToArray();

var hourlyRates = new Dictionary<string, decimal>();

for (var i = 0; i < standardUnitRates.Count - 1; i++)
{
    hourlyRates.Add(
        $"{rates[i].ValidFrom} - {rates[i + 1].ValidTo}",
        rates[i].ValueExcVat + rates[i + 1].ValueExcVat
    );
}

var cheapestRate = decimal.MaxValue;

foreach (var hourlyRate in hourlyRates.Where(hourlyRate => hourlyRate.Value < cheapestRate))
{
    cheapestRate = hourlyRate.Value;
}

Console.WriteLine(cheapestRate);
