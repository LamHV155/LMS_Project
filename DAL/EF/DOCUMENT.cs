namespace DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENT")]
    public partial class DOCUMENT
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(50)]
        public string TITLE { get; set; }

        [StringLength(200)]
        public string DESCRIPTION { get; set; }

        [StringLength(200)]
        public string LINK { get; set; }

        [StringLength(20)]
        public string TYPE { get; set; }

        public int? TOPIC_ID { get; set; }

        public virtual TOPIC TOPIC { get; set; }
    }
}