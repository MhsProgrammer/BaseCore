using System.ComponentModel.DataAnnotations;


namespace BaseCore.Domain.Common
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
