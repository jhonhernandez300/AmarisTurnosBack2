using Core.Interfaces;
using Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EF.Repositories
{
    public class UsuarioRepositorio : BaseRepositorio<Usuario>, IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _context;        

        public UsuarioRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;            
        }

        //public async Task<Usuario> GetByIdAsync(string id)
        //{
        //    return await _context.Users.FindAsync(id);
        //}
    }
}
