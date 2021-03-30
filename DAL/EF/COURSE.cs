namespace DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("COURSE")]
    public partial class COURSE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COURSE()
        {
            TOPICs = new HashSet<TOPIC>();
            C_USER = new HashSet<C_USER>();
        }

        [StringLength(20)]
        public string ID { get; set; }

        [StringLength(50)]
        public string NAME { get; set; }

        [StringLength(200)]
        public string DESCRIPTION { get; set; }

        [StringLength(20)]
        public string SEMESTER_ID { get; set; }

        [StringLength(20)]
        public string SUBJECT_ID { get; set; }

        public virtual SEMESTER SEMESTER { get; set; }

        public virtual SUBJECT SUBJECT { get; set; }

        public virtual TEACH TEACH { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TOPIC> TOPICs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C_USER> C_USER { get; set; }
    }
}
