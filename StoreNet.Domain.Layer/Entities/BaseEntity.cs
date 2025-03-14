using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace StoreNet.Domain.Layer.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        protected BaseEntity()
        {
            // Initialisation du GUID combiné au moment de la création de l'entité.
            // Id = GenerateCombGuid();
            CreatedAt = DateTime.UtcNow;
        }

        private Guid GenerateCombGuid()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();
            byte[] timestampArray = BitConverter.GetBytes(DateTime.UtcNow.Ticks);

            // Combine les deux tableaux de bytes : le GUID et le timestamp
            Array.Copy(timestampArray, 0, guidArray, 0, timestampArray.Length);

            return new Guid(guidArray); // Retourne un nouveau GUID combiné
        }

        public void SetUpdatedAt()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
