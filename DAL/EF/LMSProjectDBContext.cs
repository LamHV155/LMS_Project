using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DAL.EF
{
    public partial class LMSProjectDBContext : DbContext
    {
        public LMSProjectDBContext()
            : base("name=LMSProjectDBContext")
        {
        }

        public virtual DbSet<C_USER> C_USER { get; set; }
        public virtual DbSet<ASSESSMENT> ASSESSMENTs { get; set; }
        public virtual DbSet<CLASS> CLASSes { get; set; }
        public virtual DbSet<COURSE> COURSEs { get; set; }
        public virtual DbSet<DOCUMENT> DOCUMENTs { get; set; }
        public virtual DbSet<EVENT> EVENTs { get; set; }
        public virtual DbSet<FACULTY> FACULTies { get; set; }
        public virtual DbSet<ROLE> ROLEs { get; set; }
        public virtual DbSet<SEMESTER> SEMESTERs { get; set; }
        public virtual DbSet<SUBJECT> SUBJECTs { get; set; }
        public virtual DbSet<SUBMIT> SUBMITs { get; set; }
        public virtual DbSet<TEACH> TEACHES { get; set; }
        public virtual DbSet<TOPIC> TOPICs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<C_USER>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<C_USER>()
                .Property(e => e.PHONE_NO)
                .IsUnicode(false);

            modelBuilder.Entity<C_USER>()
                .Property(e => e.MAIL)
                .IsUnicode(false);

            modelBuilder.Entity<C_USER>()
                .Property(e => e.PASSWORD)
                .IsUnicode(false);

            modelBuilder.Entity<C_USER>()
                .Property(e => e.CLASS_ID)
                .IsUnicode(false);

            modelBuilder.Entity<C_USER>()
                .Property(e => e.FACULTY_ID)
                .IsUnicode(false);

            modelBuilder.Entity<C_USER>()
                .HasMany(e => e.SUBMITs)
                .WithOptional(e => e.C_USER)
                .HasForeignKey(e => e.USER_ID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<C_USER>()
                .HasMany(e => e.TEACHES)
                .WithOptional(e => e.C_USER)
                .HasForeignKey(e => e.USER_ID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ASSESSMENT>()
                .Property(e => e.SUBMIT_ID)
                .IsUnicode(false);

            modelBuilder.Entity<ASSESSMENT>()
                .Property(e => e.SCORE)
                .HasPrecision(5, 2);

            modelBuilder.Entity<CLASS>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<CLASS>()
                .Property(e => e.ID_FACULTY)
                .IsUnicode(false);

            modelBuilder.Entity<CLASS>()
                .HasMany(e => e.C_USER)
                .WithOptional(e => e.CLASS)
                .HasForeignKey(e => e.CLASS_ID);

            modelBuilder.Entity<COURSE>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<COURSE>()
                .Property(e => e.SEMESTER_ID)
                .IsUnicode(false);

            modelBuilder.Entity<COURSE>()
                .Property(e => e.SUBJECT_ID)
                .IsUnicode(false);

            modelBuilder.Entity<COURSE>()
                .HasOptional(e => e.TEACH)
                .WithRequired(e => e.COURSE)
                .WillCascadeOnDelete();

            modelBuilder.Entity<COURSE>()
                .HasMany(e => e.TOPICs)
                .WithOptional(e => e.COURSE)
                .HasForeignKey(e => e.COURSE_ID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<COURSE>()
                .HasMany(e => e.C_USER)
                .WithMany(e => e.COURSEs)
                .Map(m => m.ToTable("LEARNS").MapRightKey("USER_ID"));

            modelBuilder.Entity<DOCUMENT>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<DOCUMENT>()
                .Property(e => e.LINK)
                .IsUnicode(false);

            modelBuilder.Entity<DOCUMENT>()
                .Property(e => e.TYPE)
                .IsUnicode(false);

            modelBuilder.Entity<DOCUMENT>()
                .Property(e => e.TOPIC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<EVENT>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<EVENT>()
                .Property(e => e.TOPIC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<EVENT>()
                .HasMany(e => e.SUBMITs)
                .WithOptional(e => e.EVENT)
                .HasForeignKey(e => e.EVENT_ID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<FACULTY>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<FACULTY>()
                .HasMany(e => e.C_USER)
                .WithOptional(e => e.FACULTY)
                .HasForeignKey(e => e.FACULTY_ID);

            modelBuilder.Entity<FACULTY>()
                .HasMany(e => e.CLASSes)
                .WithOptional(e => e.FACULTY)
                .HasForeignKey(e => e.ID_FACULTY);

            modelBuilder.Entity<FACULTY>()
                .HasMany(e => e.SUBJECTs)
                .WithOptional(e => e.FACULTY)
                .HasForeignKey(e => e.FACULTY_ID);

            modelBuilder.Entity<ROLE>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<ROLE>()
                .Property(e => e.ROLE1)
                .IsUnicode(false);

            modelBuilder.Entity<ROLE>()
                .HasMany(e => e.C_USER)
                .WithMany(e => e.ROLEs)
                .Map(m => m.ToTable("USER_ROLE").MapRightKey("USER_ID"));

            modelBuilder.Entity<SEMESTER>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<SEMESTER>()
                .HasMany(e => e.COURSEs)
                .WithOptional(e => e.SEMESTER)
                .HasForeignKey(e => e.SEMESTER_ID);

            modelBuilder.Entity<SUBJECT>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<SUBJECT>()
                .Property(e => e.FACULTY_ID)
                .IsUnicode(false);

            modelBuilder.Entity<SUBJECT>()
                .HasMany(e => e.COURSEs)
                .WithOptional(e => e.SUBJECT)
                .HasForeignKey(e => e.SUBJECT_ID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<SUBMIT>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<SUBMIT>()
                .Property(e => e.LINK)
                .IsUnicode(false);

            modelBuilder.Entity<SUBMIT>()
                .Property(e => e.USER_ID)
                .IsUnicode(false);

            modelBuilder.Entity<SUBMIT>()
                .Property(e => e.EVENT_ID)
                .IsUnicode(false);

            modelBuilder.Entity<SUBMIT>()
                .HasOptional(e => e.ASSESSMENT)
                .WithRequired(e => e.SUBMIT)
                .WillCascadeOnDelete();

            modelBuilder.Entity<TEACH>()
                .Property(e => e.USER_ID)
                .IsUnicode(false);

            modelBuilder.Entity<TEACH>()
                .Property(e => e.COURSE_ID)
                .IsUnicode(false);

            modelBuilder.Entity<TOPIC>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<TOPIC>()
                .Property(e => e.COURSE_ID)
                .IsUnicode(false);

            modelBuilder.Entity<TOPIC>()
                .HasMany(e => e.DOCUMENTs)
                .WithOptional(e => e.TOPIC)
                .HasForeignKey(e => e.TOPIC_ID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<TOPIC>()
                .HasMany(e => e.EVENTs)
                .WithOptional(e => e.TOPIC)
                .HasForeignKey(e => e.TOPIC_ID)
                .WillCascadeOnDelete();
        }
    }
}
