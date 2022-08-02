using Microsoft.EntityFrameworkCore.Migrations;

namespace AlgoTrading.Neural.Persistence.Database.Migrations
{
    public partial class ConnectionsCollection_02_08_2022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NodeConnections_Nodes_NodeDTOId",
                table: "NodeConnections");

            migrationBuilder.RenameColumn(
                name: "NodeDTOId",
                table: "NodeConnections",
                newName: "NeuralNetworkDTOId");

            migrationBuilder.RenameIndex(
                name: "IX_NodeConnections_NodeDTOId",
                table: "NodeConnections",
                newName: "IX_NodeConnections_NeuralNetworkDTOId");

            migrationBuilder.AddForeignKey(
                name: "FK_NodeConnections_NeuralNetworks_NeuralNetworkDTOId",
                table: "NodeConnections",
                column: "NeuralNetworkDTOId",
                principalTable: "NeuralNetworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NodeConnections_NeuralNetworks_NeuralNetworkDTOId",
                table: "NodeConnections");

            migrationBuilder.RenameColumn(
                name: "NeuralNetworkDTOId",
                table: "NodeConnections",
                newName: "NodeDTOId");

            migrationBuilder.RenameIndex(
                name: "IX_NodeConnections_NeuralNetworkDTOId",
                table: "NodeConnections",
                newName: "IX_NodeConnections_NodeDTOId");

            migrationBuilder.AddForeignKey(
                name: "FK_NodeConnections_Nodes_NodeDTOId",
                table: "NodeConnections",
                column: "NodeDTOId",
                principalTable: "Nodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
