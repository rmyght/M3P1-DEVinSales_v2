using DevInSales.Models;
using System;
using System.Collections.Generic;

namespace DevInSales.Seeds
{
    public class UserSeed
    {
        public static List<User> Seed { get; set; } = new List<User>() { new User()
        {
            Id = 1,
            Name = "Usuário Comum Filho",
            BirthDate = new DateTime(2000, 02, 01),
            Email = "usuariofilho@gmail.com",
            Password = "userfilho123",
            ProfileId = 1
        }, new User()
        {
            Id = 2,
            Name = "Usuário Comum Pai",
            BirthDate = new DateTime(1974, 4, 11),
            Email = "usuariopai@gmail.com",
            Password = "userpai123",
            ProfileId = 1
        }, new User()
        {
            Id = 3,
            Name = "Gerente de Vendas",
            BirthDate = new DateTime(1986, 3, 14),
            Email = "gerentevendas@sales.com",
            Password = "vendas123",
            ProfileId = 2
        }, new User()
        {
            Id = 4,
            Name = "Administrador Geral",
            BirthDate = new DateTime(1996, 8, 21),
            Email = "administrador@sales.com",
            Password = "adm123",
            ProfileId = 3
        }
        };
    }
}
