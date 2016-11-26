namespace TicketHunter.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Categories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Categories()
        {
            Events = new HashSet<Events>();
            SubCategories = new HashSet<SubCategories>();
        }

        [Key]
        public int EventCategoryID { get; set; }

        [StringLength(500)]
        public string EventCategoryDescription { get; set; }

        [StringLength(50)]
        public string EventCategoryName { get; set; }

        [StringLength(50)]
        public string Icon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Events> Events { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubCategories> SubCategories { get; set; }
    }
}
