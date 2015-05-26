using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {

        }

        public virtual IDbSet<Automobile> Automobils { get; set; }

        public virtual IDbSet<FualType> FualTypes { get; set; }

        public virtual IDbSet<Transit> Transits { get; set; }

        public virtual IDbSet<FuelConsume> FuelConsumes { get; set; }

        public virtual IDbSet<PaymentType> PaymentTypes { get; set; }

        public virtual IDbSet<Repair> Repairs { get; set; }

        public virtual IDbSet<Attendance> Attendancea { get; set; }

        public virtual IDbSet<OilChange> OilChanges { get; set; }

        public virtual IDbSet<Department> Departments { get; set; }

        public virtual IDbSet<Driver> Drivers { get; set; }

        public virtual IDbSet<FuelCard> FuelCards { get; set; }

        public virtual IDbSet<TrafficCard> TrafficCards { get; set; }

        public virtual IDbSet<TrafficCardType> TrafficCardTypes { get; set; }

        public virtual IDbSet<Incident> Incidents { get; set; }

        public virtual IDbSet<Damage> Damages { get; set; }

        public virtual IDbSet<Role> Roles { get; set; }

        public virtual IDbSet<Group> Groups { get; set; }

        public virtual IDbSet<GroupRole> GroupRoles { get; set; }

        public virtual IDbSet<ConsumablePart> ConsumableParts { get; set; }


        public virtual IDbSet<PartType> PartTypes { get; set; }

        public virtual IDbSet<Log> Logs { get; set; }

        public virtual IDbSet<LogDetail> LogDetails { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>()
                        .HasRequired(t => t.Transit)
                        .WithMany(t => t.Attendances)
                        .HasForeignKey(d => d.TransitID)
                        .WillCascadeOnDelete(true);


            base.OnModelCreating(modelBuilder);
        }
    }
}
