using System.Net.Http.Json;
using System.Text.Json.Serialization;
using AgileOctopus.GoogleCloud;
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
var hourlyRates = new List<Rate>();

for (var i = 0; i < standardUnitRates.Count - 1; i++)
    hourlyRates.Add(
        new Rate
        {
            ValueExcVat = rates[i].ValueExcVat + rates[i + 1].ValueExcVat,
            ValidFrom = rates[i].ValidFrom,
            ValidTo = rates[i + 1].ValidTo,
        }
    );

Rate? cheapestRate = null;

foreach (
    var rate in hourlyRates.Where(rate =>
        cheapestRate is null || rate.ValueExcVat < cheapestRate.ValueExcVat
    )
)
    cheapestRate = rate;

cheapestRate!.ValidFrom = cheapestRate.ValidFrom.ToLocalTime();
cheapestRate.ValidTo = cheapestRate.ValidTo.ToLocalTime();

Console.WriteLine(cheapestRate);

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(StandardUnitRates))]
[JsonSerializable(typeof(Token))]
internal partial class SourceGenerationContext : JsonSerializerContext;
