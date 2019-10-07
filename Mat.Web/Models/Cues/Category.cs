using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mat.Web.Models.Cues
{
    public class Category : ICategory
    {
        [Required]
        public int Id { get; set; }

        [DisplayName("Internal Name")]
        public string InternalName { get; set; }
        
        [DisplayName("Parent Category Id")]
        public int ParentCategoryId { get; set; }

        [Required]
        [DisplayName("User Friendly Title")]
        public string Title { get; set; }
    }
 }