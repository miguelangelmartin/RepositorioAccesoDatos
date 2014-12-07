using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositorioAccesoDatos
{
    public interface IRepositorio<TViewModel, TEntidad>
    {
        TViewModel Add(TViewModel modelo);
        int Borrar(int id);
        int Borrar(TViewModel modelo);
        int Borrar(Expression<Func<TEntidad, bool>> lam);
        int Actualizar(TViewModel modelo);
        List<TViewModel> Get();
        List<TViewModel> Get(Expression<Func<TEntidad, bool>> lam);
        TViewModel Get(int pk);
        TEntidad GetModelDesdeViewModel(TViewModel model);
    }
}