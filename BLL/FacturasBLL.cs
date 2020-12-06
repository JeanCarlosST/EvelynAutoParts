using DAL;
using Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BLL
{
    public class FacturasBLL
    {
        public static bool Guardar(Facturas factura)
        {
            if (!Existe(factura.FacturaId))
                return Insertar(factura);
            else
                return Modificar(factura);
        }

        private static bool Insertar(Facturas factura)
        {
            Contexto context = new Contexto();
            bool found = false;

            try
            {
                foreach(FacturasDetalle detalle in factura.FacturasDetalle)
                {
                    Productos p = ProductosBLL.Buscar(detalle.ProductoId);
                    p.Inventario -= detalle.Cantidad;
                    ProductosBLL.Modificar(p);
                }

                Vendedores v = VendedoresBLL.Buscar(factura.VendedorId);
                v.Comision += factura.Total * 0.1;
                VendedoresBLL.Modificar(v);

                context.Facturas.Add(factura);
                found = context.SaveChanges() > 0;
            }
            catch
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }

            return found;
        }

        public static bool Modificar(Facturas factura)
        {
            Contexto context = new Contexto();
            bool found = false;

            try
            {
                //Eliminar los detalles viejos
                Facturas viejaFactura = Buscar(factura.FacturaId);

                if (viejaFactura != null)
                {
                    foreach (FacturasDetalle detalle in viejaFactura.FacturasDetalle)
                    {
                        Productos p = ProductosBLL.Buscar(detalle.ProductoId);
                        p.Inventario += detalle.Cantidad;
                        ProductosBLL.Modificar(p);
                    }
                }

                //Resta la comision al vendedor
                Vendedores v = VendedoresBLL.Buscar(factura.VendedorId);
                v.Comision -= viejaFactura.Total * 0.1;
                VendedoresBLL.Modificar(v);

                context.Database.ExecuteSqlRaw($"delete from FacturasDetalle where FacturaId = {factura.FacturaId}");
                foreach (var anterior in factura.FacturasDetalle)
                {
                    context.Entry(anterior).State = EntityState.Added;
                }

                context.Entry(factura).State = EntityState.Modified;
                found = context.SaveChanges() > 0;

                //Resta la cantidad en el inventario
                foreach (FacturasDetalle detalle in factura.FacturasDetalle)
                {
                    Productos p = ProductosBLL.Buscar(detalle.ProductoId);
                    p.Inventario -= detalle.Cantidad;
                    ProductosBLL.Modificar(p);
                }

                v = VendedoresBLL.Buscar(factura.VendedorId);
                v.Comision += factura.Total * 0.1;
                VendedoresBLL.Modificar(v);
            }
            catch
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }

            return found;
        }

        public static bool Eliminar(int id)
        {
            Contexto context = new Contexto();
            bool found = false;

            try
            {
                Facturas factura = Buscar(id);

                if (factura != null)
                {
                    foreach (FacturasDetalle detalle in factura.FacturasDetalle)
                    {
                        Productos p = ProductosBLL.Buscar(detalle.ProductoId);
                        p.Inventario += detalle.Cantidad;
                        ProductosBLL.Modificar(p);
                    }

                    Vendedores v = VendedoresBLL.Buscar(factura.VendedorId);
                    v.Comision -= factura.Total * 0.1;
                    VendedoresBLL.Modificar(v);

                    context.Facturas.Remove(factura);
                    found = context.SaveChanges() > 0;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }

            return found;
        }

        public static Facturas Buscar(int id)
        {
            Contexto context = new Contexto();
            Facturas factura;

            try
            {
                factura = context.Facturas
                    .Include(o => o.FacturasDetalle)
                    .Where(o => o.FacturaId == id)
                    .SingleOrDefault();
            }
            catch
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }

            return factura;
        }

        public static bool Existe(int id)
        {
            Contexto context = new Contexto();
            bool found = false;

            try
            {
                found = context.Facturas.Any(f => f.FacturaId == id);
            }
            catch
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }

            return found;
        }

        public static List<object> GetList(string criterio, string valor, DateTime? desde, DateTime? hasta)
        {
            List<object> lista;
            Contexto contexto = new Contexto();
            
            try
            {
                var query = (
                    from f in contexto.Facturas
                    join c in contexto.Clientes on f.ClienteId equals c.ClienteId
                    join v in contexto.Vendedores on f.VendedorId equals v.VendedorId
                    join u in contexto.Usuarios on f.UsuarioId equals u.UsuarioId
                    select new
                    {
                        f.FacturaId,
                        Usuario = u.NombreUsuario,
                        Vendedor = (v.Nombres + " " + v.Apellidos),
                        Cliente = (c.Nombres + " " + c.Apellidos),
                        f.Fecha,
                        Subtotal = f.Total - f.ITBIS,
                        f.ITBIS,
                        f.Total,
                        f.Balance
                    }
                );

                if(criterio.Length != 0)
                {
                    switch (criterio)
                    {
                        case "FacturaId":
                            query = query.Where(f => f.FacturaId == Utilities.ToInt(valor));
                            break;
                        case "Cliente":
                            query = query.Where(f => f.Cliente.ToLower().Contains(valor.ToLower()));
                            break;
                        case "Vendedor":
                            query = query.Where(f => f.Vendedor.ToLower().Contains(valor.ToLower()));
                            break;
                    }
                }

                if(desde != null && hasta != null)
                {
                    query = query.Where(f => f.Fecha >= desde && f.Fecha <= hasta);
                }

                lista = query.ToList<object>();
            }
            catch
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return lista;
        }

        public static List<Facturas> GetList(Expression<Func<Facturas, bool>> criterio)
        {
            List<Facturas> lista = new List<Facturas>();
            Contexto contexto = new Contexto();
            try
            {
                lista = contexto.Facturas.Where(criterio).AsNoTracking().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return lista;
        }

    }
}
