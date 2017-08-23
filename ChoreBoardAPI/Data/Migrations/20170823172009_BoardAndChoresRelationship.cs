using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ChoreBoardAPI.Data.Migrations
{
    public partial class BoardAndChoresRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Board",
                table: "Chores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "Chores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Chores_BoardId",
                table: "Chores",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chores_Boards_BoardId",
                table: "Chores",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "BoardId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chores_Boards_BoardId",
                table: "Chores");

            migrationBuilder.DropIndex(
                name: "IX_Chores_BoardId",
                table: "Chores");

            migrationBuilder.DropColumn(
                name: "Board",
                table: "Chores");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "Chores");
        }
    }
}
