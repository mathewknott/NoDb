using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mat.Web.Models.Cues
{
    /// <summary>
    /// 
    /// </summary>
    public class Question : IQuestion
    {
        [Key]
        public int Id { get; set; }

        public int Sequence { get; set; }

        [DisplayName("Internal Name")]
        public string InternalName { get; set; }

        [Required]
        [DisplayName("User Fiendly Title")]
        public string Title { get; set; }

        [Required]
        public string Answer { get; set; }

        [Required]
        public int? CategoryId { get; set; }
    }
 }