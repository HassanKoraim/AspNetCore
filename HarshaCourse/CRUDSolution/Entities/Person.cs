using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Person domain model class 
    /// </summary>
    public class Person
    {
        [Required (ErrorMessage = "Person Id can't be blank")]
        [Key]
        public Guid PersonId { get; set; }

        [Required(ErrorMessage = "Person Name can't be blank")]
        [StringLength (40)] // = nvarchar(40)
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email can't be blank")]
        [StringLength(40)]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }
        [StringLength(10)]
        public string? Gender { get; set; }

        public Guid? CountryId { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }
        public bool ReceiveNewsLetter { get; set; }
        public string? TIN { get; set; }

        [ForeignKey(nameof(CountryId))]
        public virtual Country? Country { get; set; }
    }
}
