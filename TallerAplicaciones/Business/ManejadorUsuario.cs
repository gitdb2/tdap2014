using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.taller.aplicaciones.interfaces;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace uy.edu.ort.taller.aplicaciones.negocio
{
    public class ManejadorUsuario : IUsuario
    {
        #region singleton
        private static ManejadorUsuario instance = new ManejadorUsuario();

        private ManejadorUsuario() { }

        public static ManejadorUsuario GetInstance()
        {
            return instance;
        }
        #endregion

        public void AltaUsuario(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public void ModificarUsuario(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public void BajaUsuario(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public List<Usuario> ListarUsuarios()
        {
            throw new NotImplementedException();
        }

        public Usuario BuscarUsuario(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public bool Login(string login, string password)
        {
            throw new NotImplementedException();
        }
    }
}
