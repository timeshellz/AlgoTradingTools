using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AlgoTrading.Neural.Persistence.Database.Migrations
{
    public partial class NeuralPersistence_01_08_2022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NeuralNetworkConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NetworkName = table.Column<string>(type: "text", nullable: true),
                    HiddenLayerCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NeuralNetworkConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Layer = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NeuralNetworks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConfigurationId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NeuralNetworks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NeuralNetworks_NeuralNetworkConfigurations_ConfigurationId",
                        column: x => x.ConfigurationId,
                        principalTable: "NeuralNetworkConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NodeConnections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocalId = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    InputNodeId = table.Column<int>(type: "integer", nullable: true),
                    OutputNodeId = table.Column<int>(type: "integer", nullable: true),
                    NodeDTOId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NodeConnections_Nodes_InputNodeId",
                        column: x => x.InputNodeId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NodeConnections_Nodes_NodeDTOId",
                        column: x => x.NodeDTOId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NodeConnections_Nodes_OutputNodeId",
                        column: x => x.OutputNodeId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InputNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    NeuralNetworkDTOId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InputNodes_NeuralNetworks_NeuralNetworkDTOId",
                        column: x => x.NeuralNetworkDTOId,
                        principalTable: "NeuralNetworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InputNodes_Nodes_Id",
                        column: x => x.Id,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Neurons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivationType = table.Column<int>(type: "integer", nullable: false),
                    NeuralNetworkDTOId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Neurons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Neurons_NeuralNetworks_NeuralNetworkDTOId",
                        column: x => x.NeuralNetworkDTOId,
                        principalTable: "NeuralNetworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Neurons_Nodes_Id",
                        column: x => x.Id,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutputNeurons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    NeuralNetworkDTOId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputNeurons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutputNeurons_NeuralNetworks_NeuralNetworkDTOId1",
                        column: x => x.NeuralNetworkDTOId1,
                        principalTable: "NeuralNetworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputNeurons_Neurons_Id",
                        column: x => x.Id,
                        principalTable: "Neurons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InputNodes_NeuralNetworkDTOId",
                table: "InputNodes",
                column: "NeuralNetworkDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_NeuralNetworks_ConfigurationId",
                table: "NeuralNetworks",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_Neurons_NeuralNetworkDTOId",
                table: "Neurons",
                column: "NeuralNetworkDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_NodeConnections_InputNodeId",
                table: "NodeConnections",
                column: "InputNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_NodeConnections_NodeDTOId",
                table: "NodeConnections",
                column: "NodeDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_NodeConnections_OutputNodeId",
                table: "NodeConnections",
                column: "OutputNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputNeurons_NeuralNetworkDTOId1",
                table: "OutputNeurons",
                column: "NeuralNetworkDTOId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InputNodes");

            migrationBuilder.DropTable(
                name: "NodeConnections");

            migrationBuilder.DropTable(
                name: "OutputNeurons");

            migrationBuilder.DropTable(
                name: "Neurons");

            migrationBuilder.DropTable(
                name: "NeuralNetworks");

            migrationBuilder.DropTable(
                name: "Nodes");

            migrationBuilder.DropTable(
                name: "NeuralNetworkConfigurations");
        }
    }
}
