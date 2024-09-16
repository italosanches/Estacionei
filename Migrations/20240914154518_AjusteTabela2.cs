using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Estacionei.Migrations
{
    /// <inheritdoc />
    public partial class AjusteTabela2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
           name: "DataEntrada",
           table: "Entrada",
           nullable: false, // ou true se for permitido nulo
           oldClrType: typeof(DateTime),
           oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
          name: "DataEntrada",
          table: "Entrada",
          type: "datetime2",
          nullable: false, // ou true se for permitido nulo
          oldClrType: typeof(DateTime));
        }
    }
}
