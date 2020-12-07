﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Migrations
{
    [DbContext(typeof(Contexto))]
    partial class ContextoModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Entidades.Clientes", b =>
                {
                    b.Property<int>("ClienteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Cedula")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Celular")
                        .HasColumnType("TEXT");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nombres")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ClienteId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Clientes");

                    b.HasData(
                        new
                        {
                            ClienteId = 1,
                            Apellidos = "Perez",
                            Cedula = "05612345671",
                            Direccion = "Calle G",
                            Email = "juanp@gmail.com",
                            Fecha = new DateTime(2020, 12, 7, 14, 39, 58, 163, DateTimeKind.Local).AddTicks(1847),
                            Nombres = "Juan",
                            Telefono = "8092348079",
                            UsuarioId = 1
                        });
                });

            modelBuilder.Entity("Entidades.Cobros", b =>
                {
                    b.Property<int>("CobroId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClienteId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("TEXT");

                    b.Property<double>("Total")
                        .HasColumnType("money");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("VendedorId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CobroId");

                    b.HasIndex("ClienteId");

                    b.HasIndex("UsuarioId");

                    b.HasIndex("VendedorId");

                    b.ToTable("Cobros");
                });

            modelBuilder.Entity("Entidades.CobrosDetalle", b =>
                {
                    b.Property<int>("CobroDetalleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CobroId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FacturaId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Monto")
                        .HasColumnType("money");

                    b.HasKey("CobroDetalleId");

                    b.HasIndex("CobroId");

                    b.HasIndex("FacturaId");

                    b.ToTable("CobrosDetalle");
                });

            modelBuilder.Entity("Entidades.Facturas", b =>
                {
                    b.Property<int>("FacturaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Balance")
                        .HasColumnType("money");

                    b.Property<int>("ClienteId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("TEXT");

                    b.Property<double>("ITBIS")
                        .HasColumnType("money");

                    b.Property<double>("Total")
                        .HasColumnType("money");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VendedorId")
                        .HasColumnType("INTEGER");

                    b.HasKey("FacturaId");

                    b.HasIndex("ClienteId");

                    b.HasIndex("UsuarioId");

                    b.HasIndex("VendedorId");

                    b.ToTable("Facturas");
                });

            modelBuilder.Entity("Entidades.FacturasDetalle", b =>
                {
                    b.Property<int>("FacturaDetalleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("Cantidad")
                        .HasColumnType("REAL");

                    b.Property<double>("Descuento")
                        .HasColumnType("money");

                    b.Property<int>("FacturaId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("ITBIS")
                        .HasColumnType("money");

                    b.Property<double>("Precio")
                        .HasColumnType("money");

                    b.Property<int>("ProductoId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Total")
                        .HasColumnType("money");

                    b.HasKey("FacturaDetalleId");

                    b.HasIndex("FacturaId");

                    b.HasIndex("ProductoId");

                    b.ToTable("FacturasDetalle");
                });

            modelBuilder.Entity("Entidades.Productos", b =>
                {
                    b.Property<int>("ProductoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Costo")
                        .HasColumnType("money");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("Inventario")
                        .HasColumnType("REAL");

                    b.Property<float>("MargenGanancia")
                        .HasColumnType("REAL");

                    b.Property<float>("MaxDescuento")
                        .HasColumnType("REAL");

                    b.Property<float>("PorcentajeITBIS")
                        .HasColumnType("REAL");

                    b.Property<double>("Precio")
                        .HasColumnType("money");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProductoId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Productos");

                    b.HasData(
                        new
                        {
                            ProductoId = 1,
                            Costo = 30.0,
                            Descripcion = "Aceite",
                            Inventario = 100f,
                            MargenGanancia = 0.5f,
                            MaxDescuento = 0.4f,
                            PorcentajeITBIS = 0.18f,
                            Precio = 60.0,
                            UsuarioId = 1
                        });
                });

            modelBuilder.Entity("Entidades.Usuarios", b =>
                {
                    b.Property<int>("UsuarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Clave")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("TEXT");

                    b.Property<string>("NombreUsuario")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nombres")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UsuarioId");

                    b.ToTable("Usuarios");

                    b.HasData(
                        new
                        {
                            UsuarioId = 1,
                            Apellidos = "Santos",
                            Clave = "7523c62abdb7628c5a9dad8f97d8d8c5c040ede36535e531a8a3748b6cae7e00",
                            FechaCreacion = new DateTime(2020, 12, 7, 14, 39, 58, 160, DateTimeKind.Local).AddTicks(3028),
                            NombreUsuario = "jean",
                            Nombres = "Jean Carlos"
                        },
                        new
                        {
                            UsuarioId = 2,
                            Apellidos = "Maria",
                            Clave = "7523c62abdb7628c5a9dad8f97d8d8c5c040ede36535e531a8a3748b6cae7e00",
                            FechaCreacion = new DateTime(2020, 12, 7, 14, 39, 58, 162, DateTimeKind.Local).AddTicks(1094),
                            NombreUsuario = "david",
                            Nombres = "David"
                        });
                });

            modelBuilder.Entity("Entidades.Vendedores", b =>
                {
                    b.Property<int>("VendedorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Comision")
                        .HasColumnType("money");

                    b.Property<string>("Nombres")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("INTEGER");

                    b.HasKey("VendedorId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Vendedores");

                    b.HasData(
                        new
                        {
                            VendedorId = 1,
                            Apellidos = "Jimenez",
                            Comision = 0.0,
                            Nombres = "Paco",
                            UsuarioId = 2
                        });
                });

            modelBuilder.Entity("Entidades.Clientes", b =>
                {
                    b.HasOne("Entidades.Usuarios", null)
                        .WithMany("Clientes")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Entidades.Cobros", b =>
                {
                    b.HasOne("Entidades.Clientes", null)
                        .WithMany("Cobros")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entidades.Usuarios", null)
                        .WithMany("Cobros")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entidades.Vendedores", null)
                        .WithMany("Cobros")
                        .HasForeignKey("VendedorId");
                });

            modelBuilder.Entity("Entidades.CobrosDetalle", b =>
                {
                    b.HasOne("Entidades.Cobros", null)
                        .WithMany("Detalle")
                        .HasForeignKey("CobroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entidades.Facturas", null)
                        .WithMany("CobrosDetalle")
                        .HasForeignKey("FacturaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Entidades.Facturas", b =>
                {
                    b.HasOne("Entidades.Clientes", null)
                        .WithMany("Facturas")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entidades.Usuarios", null)
                        .WithMany("Facturas")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entidades.Vendedores", null)
                        .WithMany("Facturas")
                        .HasForeignKey("VendedorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Entidades.FacturasDetalle", b =>
                {
                    b.HasOne("Entidades.Facturas", null)
                        .WithMany("FacturasDetalle")
                        .HasForeignKey("FacturaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entidades.Productos", null)
                        .WithMany("FacturasDetalle")
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Entidades.Productos", b =>
                {
                    b.HasOne("Entidades.Usuarios", null)
                        .WithMany("Productos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Entidades.Vendedores", b =>
                {
                    b.HasOne("Entidades.Usuarios", null)
                        .WithMany("Vendedores")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Entidades.Clientes", b =>
                {
                    b.Navigation("Cobros");

                    b.Navigation("Facturas");
                });

            modelBuilder.Entity("Entidades.Cobros", b =>
                {
                    b.Navigation("Detalle");
                });

            modelBuilder.Entity("Entidades.Facturas", b =>
                {
                    b.Navigation("CobrosDetalle");

                    b.Navigation("FacturasDetalle");
                });

            modelBuilder.Entity("Entidades.Productos", b =>
                {
                    b.Navigation("FacturasDetalle");
                });

            modelBuilder.Entity("Entidades.Usuarios", b =>
                {
                    b.Navigation("Clientes");

                    b.Navigation("Cobros");

                    b.Navigation("Facturas");

                    b.Navigation("Productos");

                    b.Navigation("Vendedores");
                });

            modelBuilder.Entity("Entidades.Vendedores", b =>
                {
                    b.Navigation("Cobros");

                    b.Navigation("Facturas");
                });
#pragma warning restore 612, 618
        }
    }
}
