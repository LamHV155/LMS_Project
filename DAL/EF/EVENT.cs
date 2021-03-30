namespace DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EVENT")]
    public partial class EVENT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EVENT()
        {
            SUBMITs = new HashSet<SUBMIT>();
        }

        [StringLength(20)]
        public string ID { get; set; }

        [StringLength(50)]
        public string TITLE { get; set; }

        [StringLength(200)]
        public string DESCRIPTION { get; set; }

        public DateTime? STARTDATE { get; set; }

        public DateTime? DEADLINE { get; set; }

        [StringLength(20)]
        public string TOPIC_ID { get; set; }

        public virtual TOPIC TOPIC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SUBMIT> SUBMITs { get; set; }
    }
}
