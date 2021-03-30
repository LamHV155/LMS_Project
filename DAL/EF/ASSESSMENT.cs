namespace DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ASSESSMENT")]
    public partial class ASSESSMENT
    {
        [Key]
        [StringLength(20)]
        public string SUBMIT_ID { get; set; }

        public decimal? SCORE { get; set; }

        [StringLength(200)]
        public string COMMENT { get; set; }

        public virtual SUBMIT SUBMIT { get; set; }
    }
}
