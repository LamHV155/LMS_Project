namespace DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TEACHES")]
    public partial class TEACH
    {
        [StringLength(20)]
        public string USER_ID { get; set; }

        [Key]
        [StringLength(20)]
        public string COURSE_ID { get; set; }

        public virtual C_USER C_USER { get; set; }

        public virtual COURSE COURSE { get; set; }
    }
}
