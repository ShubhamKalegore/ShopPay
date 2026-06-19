namespace ShopPay.Application.DTOs;

public class AddressDto
{
    public int AddressId { get; set; }

    public int UserId { get; set; }

    public string AddressType { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string? State { get; set; }

    public string PostalCode { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public bool IsDefault { get; set; }
}