

namespace KTVRequestASongSystem
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class KTVDBEntities : DbContext
    {
        public KTVDBEntities()
            : base("name=KTVDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Coolect> Coolect { get; set; }
        public virtual DbSet<LocalSavePathWatch> LocalSavePathWatch { get; set; }
        public virtual DbSet<SongSingleWatch> SongSingleWatch { get; set; }
        public virtual DbSet<SongSingleSongData> SongSingleSongData { get; set; }
        public virtual DbSet<ConfigPath> ConfigPath { get; set; }
        public virtual DbSet<SingerLoveWatch> SingerLoveWatch { get; set; }
    }
}
