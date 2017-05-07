using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SakabaWeb.Models
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
            Database.Log = (x => { Debug.WriteLine(x); }); // デバッグ用ログ出力
        }

        public DbSet<Boss> Bosses { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}