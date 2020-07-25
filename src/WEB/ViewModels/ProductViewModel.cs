using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WEB.Util;

namespace WEB.ViewModel
{
    public class ProductViewModel
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "The field {0} is required.")]
        [DisplayName("Fornecedor")]
        public Guid ProviderId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(200, ErrorMessage = "The field {0} need between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(1000, ErrorMessage = "The field {0} need between {2} and {1} characters", MinimumLength = 2)]
        public string Description { get; set; }

        [DisplayName("Imagem do produto")]
        public IFormFile ImageUpload { get; set; }

        public string Image { get; set; }

        [Moeda]
        [Required(ErrorMessage = "The field {0} is required.")]
        public decimal Value { get; set; }

        [ScaffoldColumn(false)]
        public DateTime RegistrationDate { get; set; }

        [DisplayName("Active?")]
        public bool Active { get; set; }

        public ProviderViewModel Provider { get; set; }
        public IEnumerable<ProviderViewModel> Providers { get; set; }
    }
}
