﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverflowEF.Migrations
{
    public partial class AnswerDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Answers",
                columns: new[] { "Id", "Content", "QuestionId", "UserId" },
                values: new object[,]
                {
                    { 1, "You can do it in dbContext class", 1, new Guid("3b16f6ed-85e1-47c7-a466-e32bdb6fafc9") },
                    { 2, "You need to configure in Program.cs", 2, new Guid("3b16f6ed-85e1-47c7-a466-e32bdb6fafc9") },
                    { 3, "You can just configure like this:", 3, new Guid("0b72e7c5-6c7a-42ca-b6c4-687cdc937d98") },
                    { 4, "DateTimeOffset is a representation of instantaneous time.", 4, new Guid("1b55d748-2ed4-4092-a1cc-a26c430d9d5e") }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
