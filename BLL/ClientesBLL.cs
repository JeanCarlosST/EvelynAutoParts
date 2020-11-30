using Microsoft.EntityFrameworkCore;
using DAL;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BLL
{
    public class ClientesBLL
    {
        public static bool Guardar(Clientes cliente)
        {
            if (!Existe(cliente.ClienteId))
                return Insertar(cliente);
            else
                return Modificar(cliente);
        }
        private static bool Insertar(Clientes cliente)
        {
            bool paso = false;
            Contexto contexto = new Contexto();
            try
            {
                contexto.Clientes.Add(cliente);
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

        public static bool Modificar(Clientes cliente)
        {
            bool paso = false;
            Contexto contexto = new Contexto();
            try
            {
                contexto.Entry(cliente).State = EntityState.Modified;
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
                var cliente = contexto.Clientes.Find(id);
                if (cliente != null)
                {
                    contexto.Clientes.Remove(cliente);//remover la entidad
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

        public static Clientes Buscar(int id)
        {
            Contexto contexto = new Contexto();
            Clientes cliente;
            try
            {
                cliente = contexto.Clientes.Find(id);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return cliente;
        }

        public static Clientes Buscar(string cedula)
        {
            Contexto contexto = new Contexto();
            Clientes cliente;
            try
            {
                cliente = (Clientes)contexto.Clientes.Where(c => c.Cedula == cedula).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                contexto.Dispose();
            }

            return cliente;
        }

        public static bool Existe(int id)
        {
            Contexto contexto = new Contexto();
            bool encontrado = false;

            try
            {
                encontrado = contexto.Clientes.Any(c => c.ClienteId == id);
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

        public static List<Clientes> GetList(Expression<Func<Clientes, bool>> criterio)
        {
            List<Clientes> lista = new List<Clientes>();
            Contexto contexto = new Contexto();
            try
            {
                lista = contexto.Clientes.Where(criterio).AsNoTracking().ToList();
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

        public static bool Existe(int option, int id, string criterio)
        {
            Contexto contexto = new Contexto();
            bool encontrado = false;

            try
            {
                switch (option)
                {
                    case 1:
                        encontrado = contexto.Clientes.Any(c => c.Email == criterio && c.ClienteId != id);
                        break;
                    case 2:
                        encontrado = contexto.Clientes.Any(c => c.Celular == criterio && c.ClienteId != id);
                        break;
                    case 3:
                        encontrado = contexto.Clientes.Any(c => c.Cedula == criterio && c.ClienteId != id);
                        break;
                    case 4:
                        encontrado = contexto.Clientes.Any(c => c.Telefono == criterio && c.ClienteId != id);
                        break;
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
            return encontrado;
        }
    }
}
