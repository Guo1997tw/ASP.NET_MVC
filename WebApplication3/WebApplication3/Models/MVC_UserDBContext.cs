using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebApplication3.Models
{
    public partial class MVC_UserDBContext : DbContext
    {
        public MVC_UserDBContext()
            //��Ʈw�s�u�A�p�GMVC_DB�C
            : base("name=MVC_UserDB")
        {
        }

        //������ƪ�A�p�Gdbo.UserTable�C
        public virtual DbSet<UserTable> UserTables { get; set; }

        //Code First���F�Ҳ��ͥX�Ӫ��C
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTable>()
                .Property(e => e.UserSex)
                .IsFixedLength();
        }
    }
}