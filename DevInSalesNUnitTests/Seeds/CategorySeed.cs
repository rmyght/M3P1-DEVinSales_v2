﻿using DevInSales.Models;
using System.Collections.Generic;

namespace DevInSales.Seeds
{
    public class CategorySeed
    {
        public static List<Category> Seed { get; set; } = new List<Category>() { new Category(1, "Categoria Padrão", "categoria-padrao")};
    }
}
