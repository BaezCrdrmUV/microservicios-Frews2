using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MSClientes.Models
{
    public partial class clientesContext : DbContext
    {
        public clientesContext()
        {
        }

        public clientesContext(DbContextOptions<clientesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Correo> Correos { get; set; }
        public virtual DbSet<Direccione> Direcciones { get; set; }
        public virtual DbSet<Membresia> Membresias { get; set; }
        public virtual DbSet<Reporte> Reportes { get; set; }
        public virtual DbSet<Telefono> Telefonos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
                Console.WriteLine(connectionString);
                optionsBuilder.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.FromString("8.0.23-mysql"),
                    builder => {
                        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("clientes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasColumnName("nombre")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Correo>(entity =>
            {
                entity.ToTable("correos");

                entity.HasIndex(e => e.IdCliente, "fk_correos_personas_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Correo1)
                    .IsRequired()
                    .HasColumnType("varchar(320)")
                    .HasColumnName("correo")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.EsPrincipal).HasColumnName("esPrincipal");

                entity.Property(e => e.IdCliente).HasColumnName("idCliente");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Correos)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("fk_correos_personas");
            });

            modelBuilder.Entity<Direccione>(entity =>
            {
                entity.ToTable("direcciones");

                entity.HasIndex(e => e.IdCliente, "fk_direcciones_personas_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Calle)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("calle")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Ciudad)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("ciudad")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Colonia)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("colonia")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Domicilio)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("domicilio")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Estado)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("estado")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IdCliente).HasColumnName("idCliente");

                entity.Property(e => e.Pais)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("pais")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Direcciones)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("fk_direcciones_personas");
            });

            modelBuilder.Entity<Membresia>(entity =>
            {
                entity.ToTable("membresias");

                entity.HasIndex(e => e.IdCliente, "fk_membresias_clientes_idx");

                entity.HasIndex(e => e.IdCorreo, "fk_membresias_correos_idx");

                entity.HasIndex(e => e.IdTelefono, "fk_membresias_telefonos_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FechaMembresia)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaMembresia");

                entity.Property(e => e.IdCliente).HasColumnName("idCliente");

                entity.Property(e => e.IdCorreo).HasColumnName("idCorreo");

                entity.Property(e => e.IdTelefono).HasColumnName("idTelefono");

                entity.Property(e => e.Tipo).HasColumnName("tipo");

                entity.Property(e => e.UltimaRenovacion)
                    .HasColumnType("datetime")
                    .HasColumnName("ultimaRenovacion");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Membresia)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_membresias_clientes");

                entity.HasOne(d => d.IdCorreoNavigation)
                    .WithMany(p => p.Membresia)
                    .HasForeignKey(d => d.IdCorreo)
                    .HasConstraintName("fk_membresias_correos");

                entity.HasOne(d => d.IdTelefonoNavigation)
                    .WithMany(p => p.Membresia)
                    .HasForeignKey(d => d.IdTelefono)
                    .HasConstraintName("fk_membresias_telefonos");
            });

            modelBuilder.Entity<Reporte>(entity =>
            {
                entity.ToTable("reportes");

                entity.HasIndex(e => e.IdCliente, "fk_reportes_clientes_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Asunto)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasColumnName("asunto")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Folio)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasColumnName("folio")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IdCliente).HasColumnName("idCliente");

                entity.Property(e => e.Notas)
                    .IsRequired()
                    .HasColumnType("varchar(320)")
                    .HasColumnName("notas")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Reportes)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("fk_reportes_clientes");
            });

            modelBuilder.Entity<Telefono>(entity =>
            {
                entity.ToTable("telefonos");

                entity.HasIndex(e => e.IdCliente, "fk_telefonos_clientes_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EsPrincipal).HasColumnName("esPrincipal");

                entity.Property(e => e.IdCliente).HasColumnName("idCliente");

                entity.Property(e => e.Telefono1)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasColumnName("telefono")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Tipo).HasColumnName("tipo");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Telefonos)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("fk_telefonos_clientes");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
