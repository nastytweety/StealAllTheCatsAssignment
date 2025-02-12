using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StealAllTheCatsAssignment.Migrations
{
    /// <inheritdoc />
    public partial class CatTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatTag_Cats_CatsId",
                table: "CatTag");

            migrationBuilder.DropForeignKey(
                name: "FK_CatTag_Tags_TagsId",
                table: "CatTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatTag",
                table: "CatTag");

            migrationBuilder.RenameTable(
                name: "CatTag",
                newName: "CatTags");

            migrationBuilder.RenameIndex(
                name: "IX_CatTag_TagsId",
                table: "CatTags",
                newName: "IX_CatTags_TagsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatTags",
                table: "CatTags",
                columns: new[] { "CatsId", "TagsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CatTags_Cats_CatsId",
                table: "CatTags",
                column: "CatsId",
                principalTable: "Cats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatTags_Tags_TagsId",
                table: "CatTags",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatTags_Cats_CatsId",
                table: "CatTags");

            migrationBuilder.DropForeignKey(
                name: "FK_CatTags_Tags_TagsId",
                table: "CatTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatTags",
                table: "CatTags");

            migrationBuilder.RenameTable(
                name: "CatTags",
                newName: "CatTag");

            migrationBuilder.RenameIndex(
                name: "IX_CatTags_TagsId",
                table: "CatTag",
                newName: "IX_CatTag_TagsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatTag",
                table: "CatTag",
                columns: new[] { "CatsId", "TagsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CatTag_Cats_CatsId",
                table: "CatTag",
                column: "CatsId",
                principalTable: "Cats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatTag_Tags_TagsId",
                table: "CatTag",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
