using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Chinook.DomainModel
{
    public class Track : IValidatableObject
    {
        public virtual int ID { get; set; }
        [Required, StringLength(200)]
        public virtual string Name { get; set; }
        public virtual int? AlbumID { get; set; }
        public virtual int? GenreID { get; set; }
        [Range(0.01, Double.MaxValue,
               ErrorMessage = "Price must be greater than zero.")]
        public virtual decimal Price { get; set; }


        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            //if (Price <= 0) yield return
            //    new ValidationResult("Price must be greater than zero.",
            //                         new string[] { "Price" });
            //if (String.IsNullOrEmpty(Name)) yield return
            //    new ValidationResult("Name is required.",
            //                         new string[] { "Name" });
            //if (Name != null && Name.Length > 200) yield return
            //    new ValidationResult(
            //                      "Name must be less than 200 characters.",
            //                      new string[] { "Name" });
            yield break;
        }
    }
}
