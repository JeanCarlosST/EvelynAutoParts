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
    public class CobrosBLL
    {
        public static bool Guardar(Cobros cobro)
        {
            if (!Existe(cobro.CobroId))
                return Insertar(cobro);
            else
                return Modificar(cobro);
        }

        private static bool Insertar(Cobros cobro)
        {
            bool paso = false;
            Contexto contexto = new Contexto();
            try
            {
                contexto.Cobros.Add(cobro);
                foreach (var item in cobro.Detalle)
                {
                    contexto.Facturas.Find(item.FacturaId).Balance += item.Monto;   
                }
                paso = contexto.SaveChanges() > 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return paso;
        }

        private static bool Modificar(Cobros cobro)
        {
            bool paso = false;
            Contexto contexto = new Contexto();
            try
            {

                Cobros ViejoCobro = CobrosBLL.Buscar(cobro.CobroId);
                foreach (var item in ViejoCobro.Detalle)
                {
                    var p = FacturasBLL.Buscar(item.FacturaId);
                    p.Balance -= item.Monto;
                    FacturasBLL.Guardar(p);
                }

                contexto.Database.ExecuteSqlRaw($"Delete FROM CobrosDetalle Where CobroId={cobro.CobroId}");
                foreach (var anterior in cobro.Detalle)
                {
                    contexto.Entry(anterior).State = EntityState.Added;
                }
                
                foreach (var item in cobro.Detalle)
                {
                    var p = FacturasBLL.Buscar(item.FacturaId);
                    p.Balance += item.Monto;
                    FacturasBLL.Guardar(p);
                }

                contexto.Entry(cobro).State = EntityState.Modified;
                paso = contexto.SaveChanges() > 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return paso;
        }
        public static bool Eliminar(int id)
        {
            bool paso = false;
            Contexto contexto = new Contexto();
            try
            {
                var cobro = CobrosBLL.Buscar(id);

                if (cobro != null)
                {
                    foreach (var item in cobro.Detalle)
                    {
                        contexto.Facturas.Find(item.FacturaId).Balance -= item.Monto;
                    }
                    contexto.Cobros.Remove(cobro);
                    paso = contexto.SaveChanges() > 0;
                }


            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return paso;
        }
        public static Cobros Buscar(int id)
        {
            Cobros cobro = new Cobros();
            Contexto contexto = new Contexto();

            try
            {
                cobro = contexto.Cobros.Include(c => c.Detalle)
                    .Where(c => c.CobroId == id)
                    .SingleOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return cobro;
        }


        public static List<Cobros> GetList(Expression<Func<Cobros, bool>> criterio)
        {
            List<Cobros> Lista = new List<Cobros>();
            Contexto contexto = new Contexto();

            try
            {
                Lista = contexto.Cobros.Where(criterio).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }
            return Lista;
        }
        public static bool Existe(int id)
        {
            Contexto contexto = new Contexto();
            bool encontrado = false;

            try
            {
                encontrado = contexto.Cobros.Any(c => c.CobroId == id);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return encontrado;
        }
    }
}
