using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitessa.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubscriptionPlanAndUserSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserSubscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "UserSubscriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SubscriptionPlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserSubscriptions");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "UserSubscriptions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SubscriptionPlans");
        }
    }
}
