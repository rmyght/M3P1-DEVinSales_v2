using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace DevInSales.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class NewUpdateProfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Profile",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "Usuario");

            migrationBuilder.InsertData(
                table: "Profile",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 2, "Gerente" },
                    { 3, "Administrador" }
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "email", "name", "password" },
                values: new object[] { "usuariofilho@gmail.com", "Usuário Comum Filho", "userfilho123" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "email", "name", "password" },
                values: new object[] { "usuariopai@gmail.com", "Usuário Comum Pai", "userpai123" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "email", "name", "password", "ProfileId" },
                values: new object[] { "gerentevendas@sales.com", "Gerente de Vendas", "vendas123", 2 });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "email", "name", "password", "ProfileId" },
                values: new object[] { "administrador@sales.com", "Administrador Geral", "adm123", 3 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Profile",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Profile",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Profile",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "Cliente");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "email", "name", "password" },
                values: new object[] { "romeu@lenda.com", "Romeu A Lenda", "romeu123@" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "email", "name", "password" },
                values: new object[] { "gustavo_levi_ferreira@gmail.com", "Gustavo Levi Ferreira", "!romeu321" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "email", "name", "password", "ProfileId" },
                values: new object[] { "lemosluiz@gmail.com", "Henrique Luiz Lemos", "lemos$2022", 1 });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "email", "name", "password", "ProfileId" },
                values: new object[] { "tomas.paulo.aragao@hotmail.com", "Tomás Paulo Aragão", "$tpa1996", 1 });
        }
    }
}
