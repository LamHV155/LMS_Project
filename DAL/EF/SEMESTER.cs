namespace DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEMESTER")]
    public partial class SEMESTER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SEMESTER()
        {
            COURSEs = new HashSet<COURSE>();
        }

        [StringLength(20)]
        public string ID { get; set; }

        [StringLength(50)]
        public string TITLE { get; set; }

        [StringLength(200)]
        public string DESCRIPTION { get; set; }

        public DateTime? START { get; set; }

        public DateTime? END_SEM { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COURSE> COURSEs { get; set; }
    }
}
