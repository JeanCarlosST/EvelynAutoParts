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
                context.Database.ExecuteSqlRaw($"delete from FacturasDetalle where FacturaId = {factura.FacturaId}");
                foreach (var anterior in factura.FacturasDetalle)
                {
                    context.Entry(anterior).State = EntityState.Added;
                }

                context.Entry(factura).State = EntityState.Modified;
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

        public static bool Eliminar(int id)
        {
            Contexto context = new Contexto();
            bool found = false;

            try
            {
                Facturas factura = Buscar(id);

                if (factura != null)
                {
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

        public static List<object> GetList()
        {
            List<object> lista;
            Contexto contexto = new Contexto();
            
            try
            {
                lista = (
                    from f in contexto.Facturas
                    join c in contexto.Clientes   on f.ClienteId equals c.ClienteId
                    join v in contexto.Vendedores on f.VendedorId equals v.VendedorId
                    join u in contexto.Usuarios   on f.UsuarioId equals u.UsuarioId
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
                ).ToList<object>();
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

    }
}
