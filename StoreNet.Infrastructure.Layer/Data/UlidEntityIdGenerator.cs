using StoreNet.Domain.Layer.Interfaces;

namespace StoreNet.Infrastructure.Layer.Data;

// Implémentation du générateur d'ULID
public class UlidEntityIdGenerator : IEntityIdGenerator
{
    public string GenerateId()
    {
        try
        {
            return Ulid.NewUlid().ToString();
        }
        catch (Exception ex)
        {
            // Log or handle error
            throw new InvalidOperationException("Failed to generate a valid ULID.", ex);
        }
    }

}

