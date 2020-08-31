namespace Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Role: BaseEntity
    {
        public Role()
        {
            Users = new List<User>();
        }


        [Required]
        [StringLength(50)]
        [Display(Name = "‰ﬁ‘ ò«—»—")]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
