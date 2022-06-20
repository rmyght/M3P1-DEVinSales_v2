﻿using DevInSales.Models;
using System.Diagnostics.CodeAnalysis;

namespace DevInSales.Seeds
{
    [ExcludeFromCodeCoverage]
    public class ProfileSeed
    {
        public static List<Profile> Seed { get; set; } = new List<Profile>() {
            new Profile(1, "Usuario"),
            new Profile(2, "Gerente"),
            new Profile(3, "Administrador")
        };
    }
}
