using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RwaLib.MODELS
{
    public class Author
    {
        public int IDAuthor { get; set; }

        [Display(Name = "Full name")]
        [Required(ErrorMessage = "Authors full name is empty")]
        public string FullName { get; set; }

        [Display(Name = "Picture path")]
        [Required(ErrorMessage = "Picture path of author is empty")]
        public string PicturePath { get; set; }

        [Required(ErrorMessage = "Description about the author is empty")]
        public string Description { get; set; }
    }
}
