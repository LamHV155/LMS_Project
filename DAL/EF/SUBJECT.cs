namespace DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SUBJECT")]
    public partial class SUBJECT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SUBJECT()
        {
            COURSEs = new HashSet<COURSE>();
        }

        [StringLength(20)]
        public string ID { get; set; }

        [StringLength(50)]
        public string NAME { get; set; }

        [StringLength(200)]
        public string DESCRIPTION { get; set; }

        [StringLength(20)]
        public string FACULTY_ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COURSE> COURSEs { get; set; }

        public virtual FACULTY FACULTY { get; set; }
    }
}
