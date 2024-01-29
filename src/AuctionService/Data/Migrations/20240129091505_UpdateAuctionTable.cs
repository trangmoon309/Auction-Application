﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuctionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Milegae",
                table: "Items",
                newName: "Mileage");

            migrationBuilder.AddColumn<DateTime>(
                name: "AuctionEnd",
                table: "Auctions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuctionEnd",
                table: "Auctions");

            migrationBuilder.RenameColumn(
                name: "Mileage",
                table: "Items",
                newName: "Milegae");
        }
    }
}
