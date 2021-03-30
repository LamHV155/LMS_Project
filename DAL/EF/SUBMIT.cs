namespace DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SUBMIT")]
    public partial class SUBMIT
    {
        [StringLength(20)]
        public string ID { get; set; }

        [StringLength(1000)]
        public string LINK { get; set; }

        public DateTime? TIME { get; set; }

        [StringLength(20)]
        public string USER_ID { get; set; }

        [StringLength(20)]
        public string EVENT_ID { get; set; }

        public virtual C_USER C_USER { get; set; }

        public virtual ASSESSMENT ASSESSMENT { get; set; }

        public virtual EVENT EVENT { get; set; }
    }
}
