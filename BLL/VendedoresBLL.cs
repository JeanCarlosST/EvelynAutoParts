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
    public class VendedoresBLL
    {
        public static bool Guardar(Vendedores vendedor)
        {
            if (!Existe(vendedor.VendedorId))
                return Insertar(vendedor);
            else
                return Modificar(vendedor);
        }
        private static bool Insertar(Vendedores vendedor)
        {
            Contexto contexto = new Contexto();
            bool found = false;

            try
            {
                contexto.Vendedores.Add(vendedor);
                found = contexto.SaveChanges() > 0;
            }
            catch
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return found;
        }
        public static bool Modificar(Vendedores vendedor)
        {
            Contexto contexto = new Contexto();
            bool found = false;

            try
            {
                contexto.Entry(vendedor).State = EntityState.Modified;
                found = contexto.SaveChanges() > 0;
            }
            catch
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return found;
        }
        public static bool Eliminar(int id)
        {
            Contexto contexto = new Contexto();
            bool found = false;

            try
            {
                var vendedor = contexto.Vendedores.Find(id);

                if (vendedor != null)
                {
                    contexto.Vendedores.Remove(vendedor);
                    found = contexto.SaveChanges() > 0;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return found;
        }
        public static Vendedores Buscar(int id)
        {
            Contexto contexto = new Contexto();
            Vendedores vendedor;

            try
            {
                vendedor = contexto.Vendedores.Find(id);
            }
            catch
            {
                throw;

            }
            finally
            {
                contexto.Dispose();
            }

            return vendedor;
        }

        public static bool Existe(int id)
        {
            Contexto contexto = new Contexto();
            bool found = false;

            try
            {
                found = contexto.Vendedores.Any(v => v.VendedorId == id);
            }
            catch
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return found;
        }

        public static List<Vendedores> GetList(Expression<Func<Vendedores, bool>> criterio)
        {
            List<Vendedores> list = new List<Vendedores>();
            Contexto contexto = new Contexto();

            try
            {
                list = contexto.Vendedores.Where(criterio).AsNoTracking().ToList<Vendedores>();
            }
            catch
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return list;
        }
    }
}
