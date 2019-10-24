using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BFB.Web.Models
{
    public partial class BFBContext : DbContext
    {
        public BFBContext()
        {
        }

        public BFBContext(DbContextOptions<BFBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BfbGame> BfbGame { get; set; }
        public virtual DbSet<BfbGameMembers> BfbGameMembers { get; set; }
        public virtual DbSet<BfbLogin> BfbLogin { get; set; }
        public virtual DbSet<BfbRole> BfbRole { get; set; }
        public virtual DbSet<BfbUser> BfbUser { get; set; }
        public virtual DbSet<BfbUserRole> BfbUserRole { get; set; }
        public virtual DbSet<BfbUserStats> BfbUserStats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("Name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BfbGame>(entity =>
            {
                entity.HasKey(e => e.GameId)
                    .HasName("PRIMARY");

                entity.ToTable("BFB_Game");

                entity.Property(e => e.GameId).HasColumnType("int(11)");

                entity.Property(e => e.BossKills).HasColumnType("int(11)");

                entity.Property(e => e.InsertedBy).HasColumnType("date");

                entity.Property(e => e.InsertedOn).HasColumnType("date");

                entity.Property(e => e.MonsterKills).HasColumnType("int(11)");

                entity.Property(e => e.PlayerKills).HasColumnType("int(11)");

                entity.Property(e => e.UpdatedBy).HasColumnType("date");

                entity.Property(e => e.UpdatedOn).HasColumnType("date");
            });

            modelBuilder.Entity<BfbGameMembers>(entity =>
            {
                entity.HasKey(e => e.GameMemberId)
                    .HasName("PRIMARY");

                entity.ToTable("BFB_GameMembers");

                entity.HasIndex(e => e.GameId)
                    .HasName("GameId");

                entity.HasIndex(e => e.UserId)
                    .HasName("UserId");

                entity.Property(e => e.GameMemberId).HasColumnType("int(11)");

                entity.Property(e => e.BossKills).HasColumnType("int(11)");

                entity.Property(e => e.GameId).HasColumnType("int(11)");

                entity.Property(e => e.InsertedBy).HasColumnType("date");

                entity.Property(e => e.InsertedOn).HasColumnType("date");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.MonsterKills).HasColumnType("int(11)");

                entity.Property(e => e.PlayerKills).HasColumnType("int(11)");

                entity.Property(e => e.TimeAsBoss).HasColumnType("int(11)");

                entity.Property(e => e.TimeAsMonster).HasColumnType("int(11)");

                entity.Property(e => e.TimeAsPlayer).HasColumnType("int(11)");

                entity.Property(e => e.UpdatedBy).HasColumnType("date");

                entity.Property(e => e.UpdatedOn).HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.BfbGameMembers)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("BFB_GameMembers_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BfbGameMembers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("BFB_GameMembers_ibfk_2");
            });

            modelBuilder.Entity<BfbLogin>(entity =>
            {
                entity.HasKey(e => e.LoginId)
                    .HasName("PRIMARY");

                entity.ToTable("BFB_Login");

                entity.HasIndex(e => e.UserId)
                    .HasName("UserId");

                entity.Property(e => e.LoginId).HasColumnType("int(11)");

                entity.Property(e => e.InsertedBy).HasColumnType("date");

                entity.Property(e => e.InsertedOn).HasColumnType("date");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.Token).HasColumnType("varchar(50)");

                entity.Property(e => e.UpdatedBy).HasColumnType("date");

                entity.Property(e => e.UpdatedOn).HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BfbLogin)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("BFB_Login_ibfk_1");
            });

            modelBuilder.Entity<BfbRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PRIMARY");

                entity.ToTable("BFB_Role");

                entity.Property(e => e.RoleId).HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.InsertedBy).HasColumnType("date");

                entity.Property(e => e.InsertedOn).HasColumnType("date");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.UpdatedBy).HasColumnType("date");

                entity.Property(e => e.UpdatedOn).HasColumnType("date");
            });

            modelBuilder.Entity<BfbUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("BFB_User");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.EmailToken).HasColumnType("varchar(50)");

                entity.Property(e => e.InsertedOn).HasColumnType("date");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.IsBanned)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.IsVerified)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.UpdatedBy).HasColumnType("date");

                entity.Property(e => e.UpdatedOn).HasColumnType("date");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnType("varchar(20)");
            });

            modelBuilder.Entity<BfbUserRole>(entity =>
            {
                entity.HasKey(e => e.UserRoleId)
                    .HasName("PRIMARY");

                entity.ToTable("BFB_UserRole");

                entity.HasIndex(e => e.RoleId)
                    .HasName("RoleId");

                entity.HasIndex(e => e.UserId)
                    .HasName("UserId");

                entity.Property(e => e.UserRoleId).HasColumnType("int(11)");

                entity.Property(e => e.InsertedBy).HasColumnType("date");

                entity.Property(e => e.InsertedOn).HasColumnType("date");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.RoleId).HasColumnType("int(11)");

                entity.Property(e => e.UpdatedBy).HasColumnType("date");

                entity.Property(e => e.UpdatedOn).HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.BfbUserRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("BFB_UserRole_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BfbUserRole)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("BFB_UserRole_ibfk_1");
            });

            modelBuilder.Entity<BfbUserStats>(entity =>
            {
                entity.HasKey(e => e.UserStatId)
                    .HasName("PRIMARY");

                entity.ToTable("BFB_UserStats");

                entity.HasIndex(e => e.UserId)
                    .HasName("UserId");

                entity.Property(e => e.UserStatId).HasColumnType("int(11)");

                entity.Property(e => e.GamesPlayed).HasColumnType("int(11)");

                entity.Property(e => e.InsertedBy).HasColumnType("date");

                entity.Property(e => e.InsertedOn).HasColumnType("date");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.UpdatedBy).HasColumnType("date");

                entity.Property(e => e.UpdatedOn).HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BfbUserStats)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("BFB_UserStats_ibfk_1");
            });
        }
    }
}
