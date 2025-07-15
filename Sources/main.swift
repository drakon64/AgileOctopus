import AsyncHTTPClient
import Foundation

struct StandardUnitRates: Decodable {
    let results: [Result]
}

struct Result: Decodable {
    let valueIncVat: Double
    let validFrom, validTo: Date

    enum CodingKeys: String, CodingKey {
        case valueIncVat = "value_inc_vat"
        case validFrom = "valid_from"
        case validTo = "valid_to"
    }
}

let decoder = JSONDecoder()
decoder.dateDecodingStrategy = .iso8601

let httpClient = HTTPClient(eventLoopGroupProvider: .singleton)

let request = HTTPClientRequest(url: "https://api.octopus.energy/v1/products/AGILE-FLEX-22-11-25/electricity-tariffs/E-1R-AGILE-FLEX-22-11-25-C/standard-unit-rates/?period_from=2023-03-26T00:00Z&period_to=2023-03-26T01:29Z")
let response = try await httpClient.execute(request, timeout: .seconds(INT64_MAX))
let body = try await response.body.collect(upTo: Int(INT_MAX))
httpClient.shutdown()

let standardUnitRates = try decoder.decode(StandardUnitRates.self, from: body)
print(standardUnitRates)
