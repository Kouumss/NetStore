﻿using System.Text.Json.Serialization;


namespace StoreNet.Domain.Layer.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentStatus
{
    Pending = 1,
    Completed = 6,
    Failed = 7,
    Refunded = 10
}
