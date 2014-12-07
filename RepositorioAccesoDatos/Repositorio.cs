using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositorioAccesoDatos
{
    public class Repositorio<TViewModel, TEntidad> :
        IRepositorio<TViewModel, TEntidad>
        where TViewModel : class,IViewModel<TEntidad>, new()
        where TEntidad : class
    {

        //protected WebApiTelefonos.Models.VentaTelefonoEntities Context;
        protected DbContext Context;

        //public Repositorio(WebApiTelefonos.Models.VentaTelefonoEntities context)
        public Repositorio(DbContext context)
        {
            Context = context;
        }

        protected DbSet<TEntidad> DbSet
        {
            get { return Context.Set<TEntidad>(); }
        }

        public virtual TViewModel Add(TViewModel modelo)
        {
            var m = modelo.ToBaseDatos();

            DbSet.Add(m);

            try
            {
                Context.SaveChanges();
                modelo.FromBaseDatos(m);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            return modelo;
        }

        public virtual int Borrar(int id)
        {

            var mod = DbSet.Find(id);
            DbSet.Remove(mod);
            int n = 0;
            try
            {
                n = Context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return n;
        }

        public int Borrar(TViewModel modelo)
        {
            var dato = GetModelDesdeViewModel(modelo);

            DbSet.Remove(dato);
            int n = 0;
            try
            {
                n = Context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return n;
        }

        public virtual int Borrar(Expression<Func<TEntidad, bool>> lam)
        {
            var datos = DbSet.Where(lam);
            DbSet.RemoveRange(datos);
            int n = 0;
            try
            {
                n = Context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return n;

        }

        public virtual int Actualizar(TViewModel modelo)
        {
            var datos = GetModelDesdeViewModel(modelo);

            modelo.UpdateBaseDatos(datos);


            int n = 0;
            try
            {
                n = Context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return n;

        }

        public TEntidad GetModelDesdeViewModel(TViewModel modelo)
        {
            var pks = modelo.GetPk();
            TEntidad datos = null;
            if (pks.Length == 1)
                datos = DbSet.Find(pks[0]);
            else if (pks.Length == 2)
                datos = DbSet.Find(pks[0], pks[1]);
            else if (pks.Length == 3)
                datos = DbSet.Find(pks[0], pks[1], pks[2]);
            else if (pks.Length == 4)
                datos = DbSet.Find(pks[0], pks[1], pks[2], pks[3]);

            return datos;
        }

        public virtual List<TViewModel> Get()
        {
            var datos = DbSet;

            List<TViewModel> list = new List<TViewModel>();

            foreach (var entidad in datos)
            {
                var v = new TViewModel();
                v.FromBaseDatos(entidad);
                list.Add(v);
            }


            return list;
        }

        public virtual List<TViewModel> Get(Expression<Func<TEntidad, bool>> lam)
        {
            var datos = DbSet.Where(lam);

            List<TViewModel> list = new List<TViewModel>();

            foreach (var entidad in datos)
            {
                var v = new TViewModel();
                v.FromBaseDatos(entidad);
                list.Add(v);
            }


            return list;
        }

        public virtual TViewModel Get(int pk)
        {
            var v = new TViewModel();
            var entidad = DbSet.Find(pk);
            v.FromBaseDatos(entidad);
            return v;
        }
    }
}