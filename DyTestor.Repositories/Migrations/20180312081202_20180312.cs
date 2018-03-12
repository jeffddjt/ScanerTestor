using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DyTestor.Repositories.Migrations
{
    public partial class _20180312 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Line",
                table: "QRCode",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "QRCode",
                newName: "AssemblyLine");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "QRCode",
                newName: "Line");

            migrationBuilder.RenameColumn(
                name: "AssemblyLine",
                table: "QRCode",
                newName: "Content");
        }
    }
}
