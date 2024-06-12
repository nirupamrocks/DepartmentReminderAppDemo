using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DepartmentReminderAppDemo.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public byte[] Logo { get; set; }

        public int? ParentDepartmentId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [ForeignKey("ParentDepartmentId")]
        public virtual Department ParentDepartment { get; set; }

        public virtual ICollection<Department> SubDepartments { get; set; } = new List<Department>();
    }
}
