
using System.Text.Json.Serialization;

namespace StoreNet.Domain.Layer.Entities
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AddressType
    {
        Billing = 1,
        Shipping = 2
    }
}
