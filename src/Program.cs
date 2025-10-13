using System.Net.Http.Json;
using AgileOctopus.Octopus;

var now = DateTime.Now.AddDays(1).Date;

var validFrom = now.ToUniversalTime().ToString("O");

var validTo = now.Add(new TimeSpan(hours: 23, minutes: 59, seconds: 0))
    .ToUniversalTime()
    .ToString("O");

var httpClient = new HttpClient();

var standardUnitRates = (
    await httpClient.GetFromJsonAsync<StandardUnitRates>(
        $"https://api.octopus.energy/v1/products/AGILE-24-10-01/electricity-tariffs/E-1R-AGILE-24-10-01-N/standard-unit-rates/?period_from={validFrom}&period_to={validTo}",
        OctopusSourceGenerationContext.Default.StandardUnitRates
    )
)!;

var rates = standardUnitRates.Results.Reverse().ToArray();
var hourlyRates = new List<Rate>(standardUnitRates.Count - 1);

for (var i = 0; i < standardUnitRates.Count - 1; i++)
    hourlyRates.Add(
        new Rate
        {
            ValueExcVat = (rates[i].ValueExcVat + rates[i + 1].ValueExcVat) / 2,
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

httpClient.Dispose();
