using System.Net.Http.Json;
using System.Text;
using AgileOctopus.Octopus;
using AgileOctopus.Twilio;

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

var accountSid =
    Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID")
    ?? throw new InvalidOperationException("TWILIO_ACCOUNT_SID is null");

var apiKey =
    Environment.GetEnvironmentVariable("TWILIO_API_KEY")
    ?? throw new InvalidOperationException("TWILIO_API_KEY is null");

var apiKeySecret = Convert.ToBase64String(
    Encoding
        .GetEncoding("ISO-8859-1")
        .GetBytes(
            Environment.GetEnvironmentVariable("TWILIO_API_KEY_SECRET")
                ?? throw new InvalidOperationException("TWILIO_API_KEY_SECRET is null")
        )
);

await httpClient.SendAsync(
    new HttpRequestMessage
    {
        RequestUri = new Uri(
            $"https://api.twilio.com/2010-04-01/Accounts/{accountSid}/Messages.json"
        ),
        Content = JsonContent.Create(
            new Message
            {
                From = "",
                To = "",
                Body = "",
            },
            TwilioSourceGenerationContext.Default.Message
        ),
        Headers = { { "Authorization", $"Basic {apiKey}:{apiKeySecret}" } },
        Method = HttpMethod.Post,
    }
);

httpClient.Dispose();
