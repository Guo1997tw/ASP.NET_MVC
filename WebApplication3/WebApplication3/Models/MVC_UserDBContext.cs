using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebApplication3.Models
{
    public partial class MVC_UserDBContext : DbContext
    {
        public MVC_UserDBContext()
            //資料庫連線，如：MVC_DB。
            : base("name=MVC_UserDB")
        {
        }

        //對應資料表，如：dbo.UserTable。
        public virtual DbSet<UserTable> UserTables { get; set; }

        //Code First精靈所產生出來的。
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTable>()
                .Property(e => e.UserSex)
                .IsFixedLength();
        }
    }
}