using BaseCore.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Domain.Entities
{
    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(300)]
        [DisplayName("نام محصول")]
        public string PorudctName { get; set; } = string.Empty;



    }
}
