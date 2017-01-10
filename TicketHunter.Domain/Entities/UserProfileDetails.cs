namespace TicketHunter.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserProfileDetails")]
    public partial class UserProfileDetails
    {
            [Key]
            public int UserProfileDetailsID { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string TelephoneNumber { get; set; }

            public string Id { get; set; }
    }
}
