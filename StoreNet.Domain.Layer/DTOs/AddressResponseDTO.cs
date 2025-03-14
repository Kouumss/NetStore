﻿namespace StoreNet.Domain.Layer.DTOs;

// DTO for returning address details.
public class AddressResponseDTO
{
    public string Id { get; set; }
    public string CustomerId { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public string AddressType { get; set; }
}
